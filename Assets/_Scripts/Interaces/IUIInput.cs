using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentEvilClone
{
    public interface IUIInput
    {
        bool IsTogglePauseDown();
        bool IsToggleInventoryDown();
        bool IsCancelDown();
    }
}
