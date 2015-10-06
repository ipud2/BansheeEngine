﻿using System.Collections.Generic;
using BansheeEngine;

namespace BansheeEditor
{
    /// <summary>
    /// Default implementation of the inspector used when no specified inspector is provided for the type. Inspector 
    /// displays GUI for all the inspectable fields in the object.
    /// </summary>
    internal sealed class GenericInspector : Inspector
    {
        private bool isEmpty = true;
        private List<InspectableField> inspectableFields = new List<InspectableField>();

        /// <inheritdoc/>
        protected internal override void Initialize()
        {
            if (InspectedObject != null)
            {
                int currentIndex = 0;
                SerializableObject serializableObject = new SerializableObject(InspectedObject.GetType(), InspectedObject);
                foreach (var field in serializableObject.Fields)
                {
                    if (!field.Inspectable)
                        continue;

                    InspectableField inspectableField = InspectableField.CreateInspectable(field.Name, currentIndex, 0,
                        new InspectableFieldLayout(Layout), field.GetProperty());

                    inspectableFields.Add(inspectableField);
                    isEmpty = false;

                    currentIndex += inspectableField.GetNumLayoutElements();
                }

                base.SetVisible(!isEmpty);
            }
        }

        /// <inheritdoc/>
        protected internal override void Refresh()
        {
            int currentIndex = 0;
            foreach (var field in inspectableFields)
            {
                field.Refresh(currentIndex);
                currentIndex += field.GetNumLayoutElements();
            }
        }

        /// <inheritdoc/>
        internal override void SetVisible(bool visible)
        {
            base.SetVisible(!isEmpty && visible);
        }
    }
}
