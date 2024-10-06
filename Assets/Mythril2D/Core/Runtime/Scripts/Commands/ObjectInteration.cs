using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Gyvr.Mythril2D
{

    // Interface for light-specific behavior
    public interface IObjectAction
    {
        void SetState(EObjectAction state);
    }

    [Serializable]
    public class ObjectInteration : ICommand
    {
        [SerializeField] private GameObject m_object_action;
        [SerializeField] private EObjectAction m_action = EObjectAction.Switch;

        // Executes the custom function of the target object class that contains "IObjectAction" interface
        public void Execute()
        {
            IObjectAction actionObject = m_object_action.GetComponent<IObjectAction>();
            Debug.Assert(actionObject != null);
            actionObject.SetState(m_action);
        }
    }
}