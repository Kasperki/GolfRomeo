using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditable
{
    void OnHover();
    void OnBlur();
    void OnSelect(Transform target);
}
