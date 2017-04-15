using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEditable
{
    void OnHover();
    void OnBlur();
    void Move(Transform target, float rotationDelta);
    void OnSelect(bool selected, Transform target);
    void OnDelete();
}
