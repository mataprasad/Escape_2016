﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;

public class KeyboardController : MonoBehaviour
{

    public static KeyboardController instance;

    public enum EControllerState
    {
        e_InGame,
        e_UI,
        e_InConversation,
        e_ReadingDoc,
        e_InPuzzle,
        e_InventoryBar, 
        e_GameMenu
    }

    public GameObject flashLight;

    private Stack<EControllerState> m_eStateStack = new Stack<EControllerState>();

    // Use this for initialization
    void Start()
    {
        Assert.IsNull(instance);
        instance = this;

        m_eStateStack.Push(EControllerState.e_InGame);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (m_eStateStack.Peek())
            {
                case EControllerState.e_InGame:
                    {
                        IconActions.instance.ClickList();
                        break;
                    }

                case EControllerState.e_UI:
                    {
                        IconActions.instance.ClickClose();
                        break;
                    }

                case EControllerState.e_ReadingDoc:
                    {
                        Assert.IsNotNull(Document.currentDoc, "No document shown at the moment");
                        Document.currentDoc.CloseDocument();
                        break;
                    }

                case EControllerState.e_InventoryBar:
                    {
                        Inventory.instance.HideInventory();
                        break;
                    }

                case EControllerState.e_InConversation:
                    {
                        break;
                    }

                case EControllerState.e_InPuzzle:
                    {
                        PuzzlesPanel.instance.ExitPuzzle();
                        break;
                    }

                case EControllerState.e_GameMenu:
                    {
                        GameMenu.instance.Hide();
                        break;
                    }

                default:
                    {
                        Debug.LogError("Undefined control state");
                        break;
                    }
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            FlashLight.instance.SwitchOnOff(!FlashLight.instance.IsOn);
        }
    }

    public void SetState(EControllerState p_newState)
    {
        Assert.AreNotEqual(p_newState, EControllerState.e_InGame, "Push error : In game state should always be at the bottom");
        if (!m_eStateStack.Contains(p_newState))
            m_eStateStack.Push(p_newState);
    }

    public void GoBackState()
    {
        if (m_eStateStack.Pop() ==  EControllerState.e_InGame)
        {
            Debug.LogError("Pop error : In game state should always be at the bottom");
            m_eStateStack.Push(EControllerState.e_InGame);
        }
    }
}