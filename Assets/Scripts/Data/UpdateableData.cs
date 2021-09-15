using System;
using UnityEngine;

public class UpdateableData : ScriptableObject
{
   public event Action OnValuesUpdated;
   public bool autoUpdate;

   protected virtual void OnEnable() {
      
      if (autoUpdate) {
         //UnityEditor.EditorApplication.update += NotifyOfUpdatedValues;
      }
   }
   

   public void NotifyOfUpdatedValues() {
      //UnityEditor.EditorApplication.update -= NotifyOfUpdatedValues;
      if (OnValuesUpdated != null) {
         //Debug.Log("Vizow");
         OnValuesUpdated ();
      }
   }
}
