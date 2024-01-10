using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public class UIInputListener : MonoBehaviour
    {
        private IUIInput m_Input;

        public PncInventory m_Inventory;
        private PauseText m_Pause;
        private VolumeControl m_Options;

        // Update is called once per frame
        void Update()
        {          
            if (!PauseController.Instance.IsPaused)
            {
                if (m_Input == null)
                    m_Input = GetComponent<IUIInput>();
                                                                                                          
                if (m_Input != null)
                {
                    if (m_Input.IsToggleInventoryDown())
                    {
                        if (!m_Inventory)
                            m_Inventory = GetComponentInChildren<PncInventory>(true);

                        m_Inventory?.Show();
                    }
                }
            }
        }
    }
}
