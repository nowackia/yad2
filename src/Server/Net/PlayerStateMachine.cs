using System;
using System.Collections.Generic;
using System.Text;

namespace Yad.Net.Server {
    public enum MenuState : byte {
        Unlogged = 0,
        Logged,
        Chat,
        GameBrowse,
        Invalid
    }

    public enum MenuAction : byte {
        Login = 0,
        ChatEntry,
        GameBrowseEnter,
        GrameBrowseLeave
    }


    public class PlayerStateMachine {
        static MenuState[,] _transitions;

        private PlayerStateMachine() {
        }

        static PlayerStateMachine() {
            int lengthMenuState = Enum.GetValues(typeof(MenuState)).Length - 1; //without MenuState.Invalid
            int lengthMenuAction = Enum.GetValues(typeof(MenuAction)).Length;

            _transitions = new MenuState[lengthMenuState, lengthMenuAction];

            for (byte i = 0; i < (byte)lengthMenuState; ++i)
                for (byte j = 0; j < (byte)lengthMenuAction; ++j)
                {
                    MenuState state = (MenuState)(i);

                    MenuAction action = (MenuAction)(j);
                    switch(state) {
                        case MenuState.Unlogged:
                            switch(action) {
                                case MenuAction.Login:
                                    _transitions[i,j] = MenuState.Logged;
                                    break;

                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                        case MenuState.Logged:
                            switch (action) {
                                case MenuAction.ChatEntry:
                                    _transitions[i, j] = MenuState.Chat;
                                    break;
                                case MenuAction.GameBrowseEnter:
                                    _transitions[i, j] = MenuState.GameBrowse;
                                    break;
                                default:
                                    _transitions[i, j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                        case MenuState.Chat:
                            switch(action) {
                                case MenuAction.GameBrowseEnter:
                                    _transitions[i,j] = MenuState.GameBrowse;
                                    break;

                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;

                        case MenuState.GameBrowse:
                            switch(action) {
                                case MenuAction.GrameBrowseLeave:
                                    _transitions[i,j] = MenuState.Chat;
                                    break;

                                default:
                                    _transitions[i,j] = MenuState.Invalid;
                                    break;
                            }
                            break;
                    }
                }
        }
 
        public static MenuState Transform(MenuState state, MenuAction action) {
            return _transitions[(int)state,(int)action];
        }
    }
}
