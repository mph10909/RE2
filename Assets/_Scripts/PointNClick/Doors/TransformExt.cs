using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExt
{ 
    public static void SetPosition(this Transform transfrom, Transform original)
    {
        original.position = transfrom.position;
        original.localEulerAngles = transfrom.localEulerAngles;

    }

    public static void GetPosition(this Transform transfrom, Transform original)
    {
        transfrom.position = new Vector3(original.position.x, original.position.y, original.position.z);
        transfrom.localEulerAngles = new Vector3(original.localEulerAngles.x, original.localEulerAngles.y, original.localEulerAngles.z);

    }

    public static void RotateTowards(this Transform self, Transform target, float rotateSpeed)
    {
        Vector3 direction = (target.position - self.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        self.rotation = Quaternion.RotateTowards(self.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    public static Collider FindCollider(this GameObject gameObject)
    {
        Collider collider = gameObject.GetComponent<BoxCollider>();

        if (collider == null)
        {
            collider = gameObject.GetComponent<CapsuleCollider>();
        }

        if (collider == null)
        {
            collider = gameObject.GetComponent<SphereCollider>();
        }

        return collider;
    }

    public static T GetComponentFamily<T>(this GameObject gameObject) where T : Component
    {
        // Check in the current GameObject
        T component = gameObject.GetComponent<T>();
        if (component != null)
        {
            return component;
        }

        // Check in the children
        component = gameObject.GetComponentInChildren<T>();
        if (component != null)
        {
            return component;
        }

        // Check in the parent
        return gameObject.GetComponentInParent<T>();
    }
}
