//
// ExampleView.cs
//
// Author(s):
//       Josh Montoute <josh@thinksquirrel.com>
//
// Copyright (c) 2012-2014 Thinksquirrel Software, LLC
//

// Disable warnings for inspector variables.
#pragma warning disable 0649

using UnityEngine;

#if !UNITY_3_5
namespace Thinksquirrel.WordGameBuilderExample
{
#endif
    /// <summary>
    /// This class is responsible for rendering the UI.
    /// </summary>
    [AddComponentMenu("WGB Example Project/UI/Example View")]
    public sealed class ExampleView : MonoBehaviour
    {
        [SerializeField] ExampleViewModel m_ViewModel;

        // Draws the main GUI
        void OnGUI()
        {
            if (!m_ViewModel.game.isInitialized)
                return;

            GUILayout.BeginArea(new Rect(0, (Screen.height / 2) - 50, Screen.width, Screen.height - ((Screen.height / 2) - 50)));
            
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label(string.Format("Language: {0}", m_ViewModel.languages.currentLanguage));
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUI.enabled = m_ViewModel.languages.currentLanguageIsLoaded;

            for (int i = 0; i < m_ViewModel.languages.languageNames.Length; i++)
            {
                var language = m_ViewModel.languages.languageNames[i];
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(language, GUILayout.Width(120), GUILayout.Height(32)))
                {
                    m_ViewModel.languages.ChangeLanguage(language);
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            GUI.enabled = true;
            
            GUILayout.Space(20);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label(string.Format("Score: {0}", m_ViewModel.player.score));
            GUILayout.Label(string.Format(" | High Score: {0}", m_ViewModel.player.highScore));
            
            if (m_ViewModel.player.lastWordScore > 0)
            {
                GUILayout.Label(string.Format(" | Last Word: {0} ({1})", m_ViewModel.player.lastWord, m_ViewModel.player.lastWordScore));
            }
            if (m_ViewModel.player.bestWordScore > 0)
            {
                GUILayout.Label(string.Format(" | Best Word: {0} ({1})", m_ViewModel.player.bestWord, m_ViewModel.player.bestWordScore));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (m_ViewModel.game.tilePoolExists)
            {
                GUILayout.Label(string.Format("Tiles Remaining: {0}", m_ViewModel.game.tilesRemaining));
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUI.enabled = m_ViewModel.languages.currentLanguageIsLoaded && m_ViewModel.player.inputEnabled && m_ViewModel.submitButtonEnabled;
            
            if (GUILayout.Button("Submit Word", GUILayout.Width(180), GUILayout.Height(32)))
            {
                m_ViewModel.player.SubmitWord();
            }

            
            GUI.enabled = m_ViewModel.languages.currentLanguageIsLoaded && m_ViewModel.player.inputEnabled && m_ViewModel.player.hasSelection;
            
            if (GUILayout.Button("Clear Selection", GUILayout.Width(180), GUILayout.Height(32)))
            {
                m_ViewModel.player.ClearSelection();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUI.enabled = m_ViewModel.languages.currentLanguageIsLoaded;
            
            if (GUILayout.Button("Reset Tiles", GUILayout.Width(180), GUILayout.Height(32)))
            {
                m_ViewModel.game.ResetTiles();
            }

            GUI.enabled = true;

            if (GUILayout.Button(m_ViewModel.agent.automaticMode ? "Disable Automatic Mode" : "Enable Automatic Mode", GUILayout.Width(180), GUILayout.Height(32)))
            {
                m_ViewModel.agent.ToggleAutomaticMode();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (m_ViewModel.languages.currentLanguageIsLoaded)
            {
                if (m_ViewModel.player.orderedResult.hasValue)
                {
                    if (m_ViewModel.player.orderedResult.isValid)
                    {
                        GUILayout.Label(string.Format("{0} is valid: {1} points. ", m_ViewModel.player.orderedResult.input, m_ViewModel.player.orderedResult.score));
                    }
                    else
                    {
                        GUILayout.Label(m_ViewModel.player.orderedResult.input + " is");
                        GUI.color = Color.red;
                        GUILayout.Label("not");
                        GUI.color = Color.white;
                        GUILayout.Label("valid. ");
                    }
                }
                else
                {
                    if (m_ViewModel.player.hasSelection)
                    {
                        GUILayout.Label("Checking word...");
                    }
                    else
                    {
                        GUILayout.Label("No tile selected.");
                    }
                }
            }
            else
            {
                GUILayout.Label(string.Format("Loading {0}...{1}", m_ViewModel.languages.currentLanguage, m_ViewModel.languages.languageLoadProgress));
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (m_ViewModel.languages.currentLanguageIsLoaded)
            {
                if (m_ViewModel.player.orderedResult.hasValue && m_ViewModel.player.permutationResult.hasValue)
                {
                    if (m_ViewModel.player.orderedResult.isValid && m_ViewModel.player.permutationResult.isValid)
                    {
                        GUILayout.Label(string.Format("{0} permutation(s){1}",
                                                      m_ViewModel.player.permutationResult.allWords.Count,
                                                      m_ViewModel.player.multiplyByPermutations ?
                                                        string.Format(" ({0} points total):",
                                                                    m_ViewModel.player.permutationResult.score * m_ViewModel.player.permutationResult.allWords.Count) :
                                                                    ":"));

                        for (int i = 0; i < m_ViewModel.player.permutationResult.allWords.Count; i++)
                        {
                            string word = m_ViewModel.player.permutationResult.allWords[i];
                            GUILayout.Label(word);
                        }
                    }
                    else if (m_ViewModel.player.permutationResult.isValid)
                    {
                        GUILayout.Label(string.Format("{0} permutation(s) available{1}",
                                                      m_ViewModel.player.permutationResult.allWords.Count,
                                                      m_ViewModel.player.multiplyByPermutations ?
                                                        string.Format(" ({0} points total)",
                                                                    m_ViewModel.player.permutationResult.score * m_ViewModel.player.permutationResult.allWords.Count) :
                                                                    string.Empty));
                    }
                    else
                    {
                        GUILayout.Label("No valid permutations.");
                    }
                }
                else
                {
                    if (m_ViewModel.player.hasSelection)
                    {
                        GUILayout.Label("Checking word permutations...");
                    }
                    else
                    {
                        GUILayout.Label("");
                    }
                }
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            GUILayout.Label(m_ViewModel.agent.automaticMode ? m_ViewModel.agent.agentStatus : "Click a letter to select it.");
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
            
            GUILayout.EndArea();

            if (m_ViewModel.showWildcardPanel)
            {
                if (m_ViewModel.languages.currentLetters != null && m_ViewModel.languages.currentLetters.Length > 0)
                {
                    var windowPosition = new Rect(Screen.width * .125f, Screen.height * .125f, Screen.width * .75f, Screen.height * .75f);
                    GUI.Window(0, windowPosition, WildcardTilePanel, "Choose a letter");
                }
                else
                {
                    m_ViewModel.showWildcardPanel = false;
                }
            }

        }
    
        // Draws the wildcard tile panel, for selecting a wildcard tile
        void WildcardTilePanel(int id)
        {
            GUILayout.BeginScrollView(m_ViewModel.wildcardPanelScrollPosition);
            GUILayout.FlexibleSpace();
            m_ViewModel.wildcardPanelSelection = GUILayout.SelectionGrid(m_ViewModel.wildcardPanelSelection, m_ViewModel.languages.currentLetters, 5);
            GUILayout.FlexibleSpace();
            GUILayout.EndScrollView();

            if (m_ViewModel.wildcardPanelSelection != -1)
            {
                m_ViewModel.game.SelectWildcardLetter(m_ViewModel.languages.currentLetters[m_ViewModel.wildcardPanelSelection]);
            }

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Cancel", GUILayout.Width(120)))
            {
                m_ViewModel.game.SelectWildcardLetter(null);
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }
#if !UNITY_3_5
}
#endif
