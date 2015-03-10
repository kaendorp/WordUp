//
// ExampleGameLogic.cs
//
// Author(s):
//       Josh Montoute <josh@thinksquirrel.com>
//
// Copyright (c) 2012-2014 Thinksquirrel Software, LLC
//

// Disable warnings for inspector variables.
#pragma warning disable 0649

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Thinksquirrel.WordGameBuilder;
using Thinksquirrel.WordGameBuilder.Gameplay;
using Thinksquirrel.WordGameBuilder.ObjectModel;

#if !UNITY_3_5
//! Contains all components used for the example game.
namespace Thinksquirrel.WordGameBuilderExample
{
#endif
    /// <summary>
    /// This class is responsible for all game logic.
    /// </summary>
    /// <remarks>
    /// Make sure to take a look at this script!
    /// It has been thoroughly commented to help users understand how to use Word Game Builder.
    /// This example has been written to use all of the interface classes (for compatibility with user-created components).
    /// Alternatively, game logic can be written using the default components themselves.
    /// </remarks>
    [AddComponentMenu("WGB Example Project/Example Game Logic")]
    public sealed class ExampleGameLogic : WGBBase
    {
        #region Inspector variables
        // The ViewModel. Contains all data going to and coming from the user interface.
        [SerializeField] ExampleViewModel m_ViewModel;
        // The player to use for the game. This example only supports one player,
        // but Word Game builder can be used with any number of players.
        [SerializeField] GameObject m_Player;
        // The AI agent to use for automatic mode.
        [SerializeField] GameObject m_Agent;
        // The tile pools to use for the game, one per language.
        // Only one tile pool is loaded at any given time.
        [SerializeField] GameObject[] m_TilePools;
        #endregion

        #region Private variables
        // The results of the last word lookup are cached, in order to prevent additional unneeded word lookups.
        string m_WordCache;
        // A cache of the player.
        IWordGamePlayer m_PlayerCached;
        // A cache of the agent.
        IWordGameAgent m_AgentCached;
        // A cache of all tile pool objects.
        IList<ITilePool> m_TilePoolsCached;
        // The currently selected tile pool.
        ITilePool m_CurrentTilePool;
        // When the wildcard tile panel is created, the current language is cached.
        WordGameLanguage m_WildcardManagerLanguage;
        // When the wildcard tile panel is created, the current wildcard tile is cached.
        ILetterTile m_WildcardManagerTile;
        // True if a language is currently being loaded.
        bool m_LoadingLanguage;
        #endregion

        #region Unity callbacks
        void OnEnable()
        {
            // To prevent any issues in the editor, some sanity checks will go here.
            if (!m_ViewModel || !m_Player || !m_Agent || m_TilePools == null || m_TilePools.Length == 0)
            {
                WGBBase.LogError("Some scripts are not assigned in the editor!", "Word Game Builder", "ExmapleGameLogic", this);
                return;
            }

            // Get the player.
            m_PlayerCached = m_Player.GetComponentFromInterface<IWordGamePlayer>();

            if (m_PlayerCached == null)
            {
                WGBBase.LogError(string.Format("Unable to find a player named {0}!", m_PlayerCached.name), "Word Game Builder", "ExampleGameLogic", this);
                return;
            }

            // Get the agent.
            m_AgentCached = m_Agent.GetComponentFromInterface<IWordGameAgent>();

            if (m_AgentCached == null)
            {
                WGBBase.LogError(string.Format("Unable to find an agent named {0}!", m_AgentCached.name), "Word Game Builder", "ExampleGameLogic", this);
                return;
            }

            // Subscribe to UI events.
            m_ViewModel.game.onWildcardLetterSelect += OnWildcardLetterSelect;
            m_ViewModel.game.onResetTiles += OnResetTiles;
            m_ViewModel.languages.onChangeLanguage += OnChangeLanguage;
            m_ViewModel.player.onSubmitWord += OnSubmitWord;
            m_ViewModel.player.onClearSelection += OnClearSelection;
            m_ViewModel.agent.onAutomaticModeToggle += OnAutomaticModeToggle;

            // Here we get a list of languages for the UI. To do this, we need to load all language files (without decompressing them).
            // We trim any languages that don't have a matching tile pool, and any tile pools that don't have a matching language.
            WordGameLanguage.LoadAllLanguages();

            // Trimming languages...
            var languages = new List<WordGameLanguage>(WordGameLanguage.GetAllLanguages());
            var trimmedTilePoolObjects = new List<GameObject>();
            var tilePools = new List<ITilePool>();
            for(int i = languages.Count - 1; i >= 0; --i)
            {
                bool remove = true;
                for(int j = 0; j < m_TilePools.Length; ++j)
                {
                    if (!m_TilePools[j])
                        continue;

                    // Get the tile pool component from the ITilePool interface.
                    var tilePool = m_TilePools[j].GetComponentFromInterface<ITilePool>();

                    if (tilePool == null)
                        continue;

                    if (i == languages.Count - 1)
                    {
                        tilePools.Add(tilePool);
                        trimmedTilePoolObjects.Add(m_TilePools[j]);
                    }
                    if (languages[i] == tilePool.language)
                    {
                        remove = false;
                        if (i < languages.Count - 1) break;
                    }
                }

                if (remove)
                    languages.RemoveAt(i);
            }

            // Trimming tile pools...
            for(int i = tilePools.Count - 1; i >= 0; --i)
            {
                bool remove = true;
                for(int j = 0; j < languages.Count; ++j)
                {
                    // Get the tile pool component from the ITilePool interface.
                    var tilePool = tilePools[i];

                    if (tilePool.language == languages[j])
                    {
                        remove = false;
                        break;
                    }
                }
                if (remove)
                {
                    tilePools.RemoveAt(i);
                    trimmedTilePoolObjects.RemoveAt(i);
                }
            }
            m_TilePoolsCached = tilePools;
            m_TilePools = trimmedTilePoolObjects.ToArray();

            // Get a list of languages for the UI.
            m_ViewModel.languages.languageNames = languages.Select(lang => lang.languageName).ToArray();

            // Disable editing of inspector variables here.
            hideFlags = HideFlags.NotEditable;

            // We set this flag to load languages asynchronously.
            // This offloads decompression into another thread, improving performance.
            // In this example, language decompression happens during Update().
            WordGameLanguage.decompressOnLoad = false;

            if (m_TilePoolsCached.Count > 0)
            {
                // Load the first tile pool's language.
                ChangeLanguage(m_TilePoolsCached[0].language);
            }

            // Setting some UI variables.
            m_ViewModel.game.tilePoolExists = m_CurrentTilePool != null;
            m_ViewModel.game.tilesRemaining = m_ViewModel.game.tilePoolExists ? m_CurrentTilePool.Count + m_PlayerCached.heldTiles.Count : 0;

            // Let the UI know that it is ready to draw
            m_ViewModel.game.isInitialized = true;
        }
        void OnDestroy()
        {
            // Unsubscribe from UI events.
            m_ViewModel.game.onWildcardLetterSelect -= OnWildcardLetterSelect;
            m_ViewModel.game.onResetTiles -= OnResetTiles;
            m_ViewModel.languages.onChangeLanguage -= OnChangeLanguage;
            m_ViewModel.player.onSubmitWord -= OnSubmitWord;
            m_ViewModel.player.onClearSelection -= OnClearSelection;
            m_ViewModel.agent.onAutomaticModeToggle -= OnAutomaticModeToggle;
        }
        void Update()
        {
            // Setting some UI variables.
            var uiPlayer = m_ViewModel.player;
            uiPlayer.score = m_PlayerCached.score;
            uiPlayer.highScore = m_PlayerCached.highScore;
            uiPlayer.lastWord = m_PlayerCached.lastWord;
            uiPlayer.lastWordScore = m_PlayerCached.lastWordScore;
            uiPlayer.bestWord = m_PlayerCached.bestWord;
            uiPlayer.bestWordScore = m_PlayerCached.bestWordScore;
            uiPlayer.hasSelection = m_PlayerCached.selectedTiles.Count > 0;
            uiPlayer.inputEnabled = m_PlayerCached.inputEnabled;
            uiPlayer.multiplyByPermutations = m_PlayerCached.multiplyByPermutations;

            m_ViewModel.game.tilePoolExists = m_CurrentTilePool != null;
            m_ViewModel.game.tilesRemaining = m_ViewModel.game.tilePoolExists ? m_CurrentTilePool.Count + m_PlayerCached.heldTiles.Count : 0;

            // Here we check again to see if the language is loaded (this can change at any time since the language can be changed in the UI)
            m_ViewModel.languages.currentLanguageIsLoaded = WordGameLanguage.current && WordGameLanguage.current.wordSet.isExpanded;

            // If the language is loaded, check for some words
            if (m_ViewModel.languages.currentLanguageIsLoaded)
            {
                CheckWords();
            }
            // If the language has not been loaded, make sure it is not in the process of loading.
            // If it is not currently loading, start the load process
            else if (!m_LoadingLanguage && WordGameLanguage.current)
            {
                m_LoadingLanguage = true;
                WordGameLanguage.current.wordSet.DecompressAsync(OnLanguageLoad, OnLanguageProgress).SetReusable();
            }
        }
        #endregion

        #region Language loading
        // This method is called whenever the language updates its loading progress.
        void OnLanguageProgress(float progress)
        {
            // Make a nice readable string for the UI.
            m_ViewModel.languages.languageLoadProgress = progress > .001f ? progress.ToString("P0") : string.Empty;
        }
        // This method is called after the language has finished loading.
        void OnLanguageLoad()
        {
            // Set the language loading flag to false.
            m_LoadingLanguage = false;

            // Set current language letters for the UI.
            m_ViewModel.languages.currentLetters = WordGameLanguage.current.letters.Select(l => l.text ).ToArray();

            // Switch the tile pool to match the newly loaded language.
            SwitchTilePool();
        }
        // This method changes the current language.
        void ChangeLanguage(WordGameLanguage language)
        {
            // Abort all tasks before switching languages.
            // This is usually good practice.
            TaskManager.AbortAllTasksWaitForSeconds(10f);

            // Set the current language.
            WordGameLanguage.current = language;

            // Set the current language for the UI.
            m_ViewModel.languages.currentLanguage = language.languageName;

            // If a tile pool currently exists, destroy tiles to free up some memory.
            // Also remove the callback method from the tile pool.
            if (m_CurrentTilePool != null)
            {
                m_CurrentTilePool.DestroyAllTiles();
                m_CurrentTilePool.onTileDistribution -= OnTileDistribution;
            }

            // Sometimes, the language may already be loaded.
            // This usually happens when switching between scenes.
            m_ViewModel.languages.currentLanguageIsLoaded = WordGameLanguage.current.wordSet.isExpanded;
            if (m_ViewModel.languages.currentLanguageIsLoaded)
            {
                OnLanguageLoad();
            }
        }
        #endregion

        #region Tile pool management
        // This method switches the tile pool to match the current language
        void SwitchTilePool()
        {
            // Reset all player scores, records, etc.
            m_PlayerCached.ResetAllData();

            // Find the new tile pool.
            // Also add the callback method to the tile pool.
            for (int i = 0; i < m_TilePoolsCached.Count; i++)
            {
                var tilePool = m_TilePoolsCached[i];

                if (tilePool.language.Equals(WordGameLanguage.current))
                {
                    m_CurrentTilePool = tilePool;
                    m_CurrentTilePool.onTileDistribution += OnTileDistribution;
                    break;
                }
            }

            // Create a new set of letter tiles and distribute some tiles to the player.
            if (m_CurrentTilePool != null)
            {
                m_CurrentTilePool.CreateTilePool();
                m_CurrentTilePool.DistributeTiles(m_PlayerCached);
            }
        }
        /// <summary>
        /// This callback fires whenever tiles are distributed by the tile pool.
        /// </summary>
        /// <remarks>
        /// This method is assigned to the tile pool dynamically at runtime.
        /// In some cases (with permanent scene objects usually), it is easier
        /// to assign callbacks through the inspector.
        /// </remarks>
        public void OnTileDistribution()
        {
            // This shows another way to get the current player from an event callback.
            // All gameplay event callbacks have static helper properties in order to get more information.
            var player = WGBEvent.currentPlayer;
            var count = player.heldTiles.Count;

            // Move the newly distributed tiles to the player's tray, and spawn them.
            // Spawning tiles makes them visible, and allows direct interaction.
            for(int i = 0; i < count; i++)
            {
                var tile = player.heldTiles[i];

                tile.transform.localPosition = new Vector3(i * 100f - (((count * 100) / 2) - 50), 200, 0);
                tile.SpawnTile();
            }

            // If in automatic mode, find a word with the agent.
            if (m_ViewModel.agent.automaticMode)
            {
                m_ViewModel.agent.agentStatus = "Searching for words...";
                m_AgentCached.FindWords();
            }
            else
            {
                m_ViewModel.agent.agentStatus = string.Empty;
            }
        }
        #endregion

        #region Word checking
        void CheckWords()
        {
            // We cache words to prevent unneccessary lookups.
            if (m_WordCache == WordChecker.GetWord(m_PlayerCached.selectedTiles))
                return;

            m_WordCache = WordChecker.GetWord(m_PlayerCached.selectedTiles);

            // Clear any old results.
            m_ViewModel.player.orderedResult.hasValue = false;
            m_ViewModel.player.orderedResult.hasValue = false;

            // If any word check threads are running, abort them here, to prevent any race conditions.
            if (WordChecker.taskCount > 0)
            {
                WordChecker.AbortAllTasks();
            }

            // Two word checks are performed - one is an ordered word check, and one checks word permutations.
            // Different callbacks are used for each one.
            // The SetResuable method lets the task object get used again and reduces garbage allocation.
            WordChecker.CheckWordAsync(m_PlayerCached.selectedTiles, OnCheckWordOrdered).SetReusable();
            WordChecker.CheckWordAsync(m_PlayerCached.selectedTiles, OnCheckWordPermute, false).SetReusable();
        }
        void OnCheckWordOrdered(WordGameResult result)
        {
            // We only assign this result if it still matches the player's current selection.
            if (result.input == WordChecker.GetWord(m_PlayerCached.selectedTiles))
            {
                var uiResult = m_ViewModel.player.orderedResult;
                uiResult.hasValue = result.hasValue;
                uiResult.isValid = result.isValid;
                uiResult.input = result.input;
                uiResult.score = result.score;
                uiResult.allWords = result.allWords;

                // Verify if we can submit our word
                VerifyWordResults();
            }
        }
        void OnCheckWordPermute(WordGameResult result)
        {
            // We only assign this result if it still matches the player's current selection.
            if (result.input == WordChecker.GetWord(m_PlayerCached.selectedTiles))
            {
                var uiResult = m_ViewModel.player.permutationResult;
                uiResult.hasValue = result.hasValue;
                uiResult.isValid = result.isValid;
                uiResult.input = result.input;
                uiResult.score = result.score;
                uiResult.allWords = result.allWords;

                // Verify if we can submit our word
                VerifyWordResults();
            }
        }
        void VerifyWordResults()
        {
            // Here, we verify that both word results are valid.
            m_ViewModel.submitButtonEnabled = m_ViewModel.player.orderedResult.isValid && m_ViewModel.player.permutationResult.isValid;
        }
        #endregion

        #region UI callbacks
        // This method is called by the ViewModel,
        // when the user presses a button to change the language.
        void OnChangeLanguage(string language)
        {
            // Convert the language from string representation.
            WordGameLanguage lang = WordGameLanguage.FindByName(language);

            if (WordGameLanguage.current != lang)
            {
                ChangeLanguage(lang);
            }
        }
        // This method is called by the ViewModel,
        // when the user confirms a word selection.
        void OnSubmitWord()
        {
            // Disable the submission button.
            m_ViewModel.submitButtonEnabled = false;

            // Submit the word! Scoring is automatic, and based on language/player settings.
            m_PlayerCached.SubmitWord();
        }
        // This method is called by the ViewModel,
        // when the user presses a button to change a wildcard letter.
        void OnWildcardLetterSelect(string letter)
        {
            // Hide the wildcard tile selection panel.
            m_ViewModel.showWildcardPanel = false;

            // Set the letter to the currently cached tile, if the language has not changed.
            if (!string.IsNullOrEmpty(letter) && m_WildcardManagerTile != null && m_WildcardManagerLanguage == WordGameLanguage.current)
            {
                // Get a letter struct from a letter string.
                var selectedLetter = m_WildcardManagerLanguage.GetLetter(letter);

                // Set the wildcard letter. A value of 0 points is given, as well.
                m_WildcardManagerTile.SetWildcard(selectedLetter, 0);

                // Select the tile.
                m_PlayerCached.SelectTile(m_WildcardManagerTile);
            }

            m_WildcardManagerTile = null;
            m_WildcardManagerLanguage = null;
        }
        // This method is called by the ViewModel,
        // when the user presses a button to clear their current selection.
        void OnClearSelection()
        {
            // Clear the player's selection.
            m_PlayerCached.ClearSelection();
        }
        // This method is called by the ViewModel,
        // when the user presses a button to reset all tiles.
        void OnResetTiles()
        {
            // Stop the agent.
            m_AgentCached.Stop();

            // Clear the player's selection.
            m_PlayerCached.ClearSelection();

            // Clear the player's held tiles.
            m_PlayerCached.heldTiles.Clear();

            // Clear the player's score information,
            // but keep high scores.
            m_PlayerCached.ResetScore();

            if (m_CurrentTilePool != null)
            {
                // Reset the tile pool.
                m_CurrentTilePool.ResetTilePool();
                // Distribute some new tiles to the player.
                m_CurrentTilePool.DistributeTiles(m_PlayerCached);
            }
        }
        // This method is called by the View Mode,
        // when the user presses a button to toggle automatic mode.
        void OnAutomaticModeToggle()
        {
            // Toggle automatic mode
            m_ViewModel.agent.automaticMode = !m_ViewModel.agent.automaticMode;

            if (m_ViewModel.agent.automaticMode)
            {
                // Start the agent.
                m_ViewModel.agent.agentStatus = "Searching for words...";
                m_AgentCached.FindWords();
            }
            else
            {
                // Stop the agent.
                m_AgentCached.Stop();
                m_ViewModel.agent.agentStatus = string.Empty;

                // Clear the current selection.
                m_PlayerCached.ClearSelection();

                // Enable player input.
                m_PlayerCached.inputEnabled = true;
            }
        }
        #endregion

        #region Other callbacks
        /// <summary>
        /// This callback fires after the player submits a word and it has been checked by the word checker.
        /// </summary>
        /// <remarks>
        /// This method is assigned in the player's inspector.
        /// Callbacks like these can also be assigned dynamically at runtime,
        /// and anonymous delegates are supported as well.
        /// </remarks>
        public void OnPlayerWordResult()
        {
            // Again, here is another way to get the current player from within a callback.
            var result = WGBEvent.currentPlayer.lastResult;

            // If we have valid results, despawn the submitted tiles.
            if (result.isValid)
            {
                for (int i = 0; i < result.letterTiles.Count; i++)
                {
                    var tile = result.letterTiles[i];
                    tile.DespawnTile();
                }
            }
            else
            {
                // Sometimes the agent may submit invalid results,
                // depending on when it was disabled.
                // If this happens, we need to restart it.
                if (m_ViewModel.agent.automaticMode)
                {
                    m_ViewModel.agent.agentStatus = "Searching for words...";
                    m_AgentCached.FindWords();
                }
                else
                {
                    m_ViewModel.agent.agentStatus = string.Empty;
                }
            }

            // Distribute some more tiles to the plyer.
            m_CurrentTilePool.DistributeTiles(WGBEvent.currentPlayer);
        }
        /// <summary>
        /// This callback fires when the agent has finished searching for valid words.
        /// </summary>
        /// <remarks>
        /// This method is assigned in the Word Game Agent's inspector.
        /// Callbacks like these can also be assigned dynamically at runtime,
        /// and anonymous delegates are supported as well.
        /// </remarks>
        public void OnAgentFindWords()
        {
            var searchInfo = WGBEvent.currentAgent.lastSearchInfo;

            m_ViewModel.agent.agentStatus =
                !searchInfo.wasSuccessful ? "Unable to find any words..." :
                string.Format("Found {0} words! Picking {1}...", searchInfo.wordCount, searchInfo.bestWord);

        }
        /// <summary>
        /// This callback fires when a wildcard tile receives input.
        /// </summary>
        /// <remarks>
        /// This method is assigned in the Wildcard Tile Manager's inspector.
        /// Callbacks like these can also be assigned dynamically at runtime,
        /// and anonymous delegates are supported as well.
        /// </remarks>
        public void OnWildcardTileSelect()
        {
            // Get the currently selected letter tile.
            var wildcardManagerTile = WGBEvent.currentLetterTile;

            // If the letter is blank,
            // show the wildcard tile selection panel.
            if (!(wildcardManagerTile.defaultLetter.hasValue || wildcardManagerTile.wildcardLetter.hasValue))
            {
                // Show the panel and initialize UI variables.
                m_ViewModel.wildcardPanelSelection = -1;
                m_ViewModel.wildcardPanelScrollPosition = Vector2.zero;
                m_ViewModel.showWildcardPanel = true;

                // Cache manager information.
                m_WildcardManagerLanguage = WGBEvent.currentLanguage;
                m_WildcardManagerTile = wildcardManagerTile;
            }
            // If the letter is not blank, reset the wildcard value.
            else
            {
                wildcardManagerTile.RemoveWildcard();

                // Deselect the tile.
                m_PlayerCached.DeselectTile(wildcardManagerTile);
            }

        }
        #endregion
    }
#if !UNITY_3_5
}
#endif
