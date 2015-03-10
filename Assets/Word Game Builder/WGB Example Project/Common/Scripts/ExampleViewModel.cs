//
// ExampleViewModel.cs
//
// Author(s):
//       Josh Montoute <josh@thinksquirrel.com>
//
// Copyright (c) 2012-2014 Thinksquirrel Software, LLC
//
using UnityEngine;
using System.Collections.Generic;

#if !UNITY_3_5
namespace Thinksquirrel.WordGameBuilderExample
{
#endif
    /// <summary>
    /// This class is responsible for sending and receiving data between the game and the UI.
    /// <remarks>
    /// The example game follows an MVVM pattern, representing UI data in an abstract way.
    /// This class only defines data, and doesnt provide functionality on its own.
    /// \see http://en.wikipedia.org/wiki/Model_View_ViewModel
    /// </remarks>
    [AddComponentMenu("WGB Example Project/UI/Example ViewModel")]
    public sealed class ExampleViewModel : MonoBehaviour
    {
        #region Context classes
        /// <summary>
        /// Represents a base class for all user interface contexts.
        /// </summary>
        public abstract class UiContext { }
        /// <summary>
        /// Contains information about the game.
        /// </summary>
        public sealed class GameContext : UiContext
        {
            /// <summary>
            /// Is the example game initialized?
            /// </summary>
            public bool isInitialized { get; set; }
            /// <summary>
            /// Does a tile pool exist?
            /// </summary>
            public bool tilePoolExists { get; set; }
            /// <summary>
            /// The amount of tiles remaining in the game.
            /// </summary>
            public int tilesRemaining { get; set; }
        
            /// <summary>
            /// This event fires when a user presses a button to reset all tiles in the current game.
            /// </summary>
            public event System.Action onResetTiles;
            /// <summary>
            /// This event fires when a user selects a wildcard tile from the wildcard panel.
            /// </summary>
            public event System.Action<string> onWildcardLetterSelect;

            /// <summary>
            /// Reset all tiles in the current game. Called by the UI.
            /// </summary>
            public void ResetTiles()
            {
                if (onResetTiles != null)
                    onResetTiles();
            }
            /// <summary>
            /// Select a wildcard tile from the wildcard panel. Called by the UI.
            /// </summary>
            /// <param name='letter'>The letter to select (by readable name).</param>
            public void SelectWildcardLetter(string letter)
            {
                if (onWildcardLetterSelect != null)
                    onWildcardLetterSelect(letter);
            }

        }
        /// <summary>
        /// Contains information about languages.
        /// </summary>
        public sealed class LanguagesContext : UiContext
        {
            /// <summary>
            /// The current list of languages.
            /// </summary>
            public string[] languageNames { get; set; }
            /// <summary>
            /// Is the current language loaded?
            /// </summary>
            public bool currentLanguageIsLoaded { get; set; }
            /// <summary>
            /// The current language.
            /// </summary>
            public string currentLanguage { get; set; }
            /// <summary>
            /// The current language's letters.
            /// </summary>
            public string[] currentLetters { get; set; }
            /// <summary>
            /// Loading progress for the current language.
            /// </summary>
            public string languageLoadProgress { get; set; }

            /// <summary>
            /// This event fires when a language is changed by the user.
            /// </summary>
            public event System.Action<string> onChangeLanguage;

            /// <summary>
            /// Change the current language. Called by the UI.
            /// </summary>
            /// <param name='language'>The language name to change to.</param>
            public void ChangeLanguage(string language)
            {
                if (onChangeLanguage != null)
                    onChangeLanguage(language);
            }
        }
        /// <summary>
        /// Contains information about the player.
        /// </summary>
        public sealed class PlayerContext : UiContext
        {
            /// <summary>
            /// The player's current score.
            /// </summary>
            public int score { get; set; }

            /// <summary>
            /// The player's current high score.
            /// </summary>
            public int highScore { get; set; }

            /// <summary>
            /// The player's last word score.
            /// </summary>
            public int lastWordScore { get; set; }

            /// <summary>
            /// The player's best word score.
            /// </summary>
            public int bestWordScore { get; set; }

            /// <summary>
            /// The player's last word.
            /// </summary>
            public string lastWord { get; set; }

            /// <summary>
            /// The player's best word.
            /// </summary>
            public string bestWord { get; set; }

            /// <summary>
            /// Is player input enabled?
            /// </summary>
            public bool inputEnabled { get; set; }

            /// <summary>
            /// Does the player have a selection?
            /// </summary>
            public bool hasSelection { get; set; }

            /// <summary>
            /// Should the player's score be multiplied by word permutations?
            /// </summary>
            public bool multiplyByPermutations { get; set; }

            /// <summary>
            /// This event fires when a user presses a button to submit a word.
            /// </summary>
            public event System.Action onSubmitWord;
            /// <summary>
            /// This event fires when a user presses a button to clear the current tile selection.
            /// </summary>
            public event System.Action onClearSelection;

            /// <summary>
            /// Contains information about an ordered word check.
            /// </summary>
            public WordResultContext orderedResult { get; private set; }
            /// <summary>
            /// Contains information about a permutation word check.
            /// </summary>
            public WordResultContext permutationResult { get; private set; }

            /// <summary>
            /// Submit a word. Called by the UI.
            /// </summary>
            public void SubmitWord()
            {
                if (onSubmitWord != null)
                    onSubmitWord();
            }
            /// <summary>
            /// Clears the current tile selection. Called by the UI.
            /// </summary>
            public void ClearSelection()
            {
                if (onClearSelection != null)
                    onClearSelection();
            }

            public PlayerContext()
            {
                orderedResult = new WordResultContext();
                permutationResult = new WordResultContext();
            }
        }
        /// <summary>
        /// Contains information about the agent.
        /// </summary>
        public sealed class AgentContext : UiContext
        {
            /// <summary>
            /// Is automatic mode enabled?
            /// </summary>
            public bool automaticMode { get; set; }
            /// <summary>
            /// A label representing the agent's current status.
            /// </summary>
            public string agentStatus { get; set; }

            /// <summary>
            /// This event fires when the user toggles automatic mode.
            /// </summary>
            public event System.Action onAutomaticModeToggle;

            /// <summary>
            /// Toggle automatic mode. Called by the UI.
            /// </summary>
            public void ToggleAutomaticMode()
            {
                if (onAutomaticModeToggle != null)
                    onAutomaticModeToggle();
            }
        }
        /// <summary>
        /// Contains information about a word check.
        /// </summary>
        public sealed class WordResultContext : UiContext
        {
            /// <summary>
            /// Is the word check empty?
            /// </summary>
            public bool hasValue { get; set; }

            /// <summary>
            /// Is the word check valid?
            /// </summary>
            public bool isValid { get; set; }

            /// <summary>
            /// The input string for the word check.
            /// </summary>
            public string input { get; set; }

            /// <summary>
            /// The score value of the word check.
            /// </summary>
            public int score { get; set; }

            /// <summary>
            /// All words from the word check.
            /// </summary>
            public IList<string> allWords { get; set; }
        }
        #endregion

        GameContext m_Game = new GameContext();
        LanguagesContext m_Languages = new LanguagesContext();
        PlayerContext m_Player = new PlayerContext();
        AgentContext m_Agent = new AgentContext();

        /// <summary>
        /// Contains information about the game.
        /// </summary>
        public GameContext game { get { return m_Game; } }
        /// <summary>
        /// Contains information about languages.
        /// </summary>
        public LanguagesContext languages { get { return m_Languages; } }
        /// <summary>
        /// Contains information about the player.
        /// </summary>
        public PlayerContext player { get { return m_Player; } }
        /// <summary>
        /// Contains information about the agent.
        /// </summary>
        public AgentContext agent { get { return m_Agent; } }

        /// <summary>
        /// Should the submit button be enabled?
        /// </summary>
        public bool submitButtonEnabled { get; set; }
        /// <summary>
        /// Should the wildcard panel be visible?
        /// </summary>
        public bool showWildcardPanel { get; set; }
        /// <summary>
        /// The currently selected item in the wildcard panel.
        /// </summary>
        public int wildcardPanelSelection { get; set; }
        /// <summary>
        /// The scroll position of the scroll view in the wildcard panel.
        /// </summary>
        public Vector2 wildcardPanelScrollPosition { get; set; }
    }
#if !UNITY_3_5
}
#endif
