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
            
            GUI.enabled = m_ViewModel.languages.currentLanguageIsLoaded && m_ViewModel.player.inputEnabled && m_ViewModel.submitButtonEnabled;
            
            if (GUILayout.Button("Submit Word", GUILayout.Width(180), GUILayout.Height(32)))
            {
                m_ViewModel.player.SubmitWord();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.Space(10);
            
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
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
