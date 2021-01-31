using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;
using Core.Common.Extensions;
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.Composition.Hosting;
using System.Runtime.Serialization;
using Core.Common.Contracts;

namespace Core.Common.Core
{
    public abstract class ObjectBase : NotificationObject, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {

        protected IValidator _validator = null;
        protected IEnumerable<ValidationFailure> _validationErrors = null;

        //Se validan los datos directamente en el Constructor
        public ObjectBase()
        {
            _validator = GetValidator();
            Validate();
        }

        public static CompositionContainer Container { get; set; }

        #region Protected methods

        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                     Action<IList> snippetForCollection,
                                     params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;

            List<string> exemptions = new List<string>();
            if (exemptProperties != null)
                exemptions = exemptProperties.ToList();

            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);

                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (!exemptions.Contains(property.Name))
                            {
                                if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                                {
                                    ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                    walk(obj);
                                }
                                else
                                {
                                    IList collection = property.GetValue(o, null) as IList;
                                    if (collection != null)
                                    {
                                        snippetForCollection.Invoke(collection);

                                        foreach (object item in collection)
                                        {
                                            if (item is ObjectBase)
                                                walk((ObjectBase)item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            walk(this);
        }

        #endregion

        #region IDirtyCapable members

        //IsDirty nos servirá para saber si hay cambios en el objeto
        protected bool _isDirty;

        [NotNavigable]
        public bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;

            WalkObjectGraph(o =>
                            {
                                if (o.IsDirty)
                                {
                                    isDirty = true;
                                    return true; // short circuit
                                }
                                else
                                    return false;
                            }, collection => { });

            return isDirty;
        }

        //Devuelve recursivamente todos los objectos que contengan cambios
        public List<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();

            WalkObjectGraph(o =>
                            {
                                if (o.IsDirty)
                                    dirtyObjects.Add(o);

                                return false;
                            }, collection => { });

            return dirtyObjects;
        }

        //Restablece recursivamente todas las propiedades de cambio de los objetos
        public void CleanAll()
        {
            WalkObjectGraph(
            o =>
            {
                if (o.IsDirty)
                    o.IsDirty = false;

                return false;
            }, collection => { });
        }

       
        #endregion

        #region Validation

        protected virtual IValidator GetValidator()
        {
            return null;
        }

        [NotNavigable]
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get { return _validationErrors; }
            set { }
        }

        public void Validate()
        {
            if (_validator != null)
            {
                ValidationResult results = _validator.Validate(this);
                _validationErrors = results.Errors;
            }
        }

        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_validationErrors != null && _validationErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }
        #endregion
        
        #region IDataErrorInfo members
        /// <summary>
        /// Esta interfaz es llamada por XAML para validar los errores en el Binding
        /// </summary>
        string IDataErrorInfo.Error
        {
            get { return string.Empty; }
        }

        string IDataErrorInfo.this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();

                if (_validationErrors != null && _validationErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _validationErrors)
                    {
                        if (validationError.PropertyName == columnName)
                            errors.AppendLine(validationError.ErrorMessage);
                    }
                }

                return errors.ToString();
            }
        }

        #endregion

        #region Property change notification

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(true, propertyName);
        }

        protected void OnPropertyChanged(bool makeDirty, [CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (makeDirty)
                _isDirty = true;

            Validate();
        }

        #endregion

        #region IExtensibleDataObject Members

        public ExtensionDataObject ExtensionData { get; set; }

        #endregion
        
    }
}
