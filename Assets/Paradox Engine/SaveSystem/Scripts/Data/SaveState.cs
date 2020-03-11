using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ParadoxEngine.Utilities.Parameters;
using System;
using System.Linq;


namespace ParadoxEngine.Utilities.SaveSystem
{
    [System.Serializable]
    public class SaveState
    {
        #region Parameters variable
        private List<ParamReferenceVariable> _referenceData = new List<ParamReferenceVariable>();
        private List<IntSerializedParamVariable> _intData = new List<IntSerializedParamVariable>();
        private List<FloatSerializedParamVariable> _floatData = new List<FloatSerializedParamVariable>();
        private List<BoolSerializedParamVaraible> _boolData = new List<BoolSerializedParamVaraible>();
        private List<EventSerializedParamVariable> _eventData = new List<EventSerializedParamVariable>();
        #endregion

        #region Log Variable
        private List<string> _textLog = new List<string>();
        public List<string> Log => _textLog;
        #endregion

        public bool initialized = false;


        public void InitializeParameters(Params parameters)
        {
            int index;

            foreach (var param in parameters)
            {
                if (param.IsPersistent)
                {
                    _referenceData.Add(parameters[param.Name]);
                    continue;
                }

                if (param.Type == ParamVariableType.Int)
                {
                    var value = parameters.GetInt(param.Name).Value;

                    _intData.Add(new IntSerializedParamVariable() { Name = param.Name, Value = value });
                    index = _intData.Count - 1;
                }

                else if (param.Type == ParamVariableType.Float)
                {
                    var value = parameters.GetFloat(param.Name).Value;

                    _floatData.Add(new FloatSerializedParamVariable() { Name = param.Name, Value = value });
                    index = _floatData.Count - 1;
                }

                else
                {
                    var value = parameters.GetBool(param.Name).Value;

                    _boolData.Add(new BoolSerializedParamVaraible() { Name = param.Name, Value = value });
                    index = _boolData.Count - 1;
                }

                _referenceData.Add(new ParamReferenceVariable
                {
                    Name = param.Name,
                    Type = param.Type,
                    Accessibility = param.access,
                    EngineGraph = param.graph,
                    IsPersistent = false,
                    IndexInCollection = index
                });
            }

            initialized = true;
        }

        /// <summary>
        /// Add the new parameters created and remove the deleted, keeping the saved values, using the name as a reference.
        /// </summary>
        /// <param name="parameters"></param>
        public void UpdateParameters(Params parameters)
        {
            int index;

            Action remove = delegate { };

            foreach (var param in _referenceData)
            {
                if (parameters.Any(x => x.Name != param.Name))
                {
                    var temp = param;

                    remove += () =>
                    {
                        if (!temp.IsPersistent)
                        {
                            if (temp.Type == ParamVariableType.Int)
                                _intData.RemoveAt(temp.IndexInCollection);
                            else if (temp.Type == ParamVariableType.Float)
                                _floatData.RemoveAt(temp.IndexInCollection);
                            else if (temp.Type == ParamVariableType.Bool)
                                _boolData.RemoveAt(temp.IndexInCollection);
                            else
                                _eventData.RemoveAt(temp.IndexInCollection);
                        }

                        _referenceData.Remove(temp);
                    };
                }
            }

            remove();

            foreach (var param in parameters)
            {
                if (_referenceData.Any(x => x.Name == param.Name))
                    continue;

                if (param.IsPersistent)
                {
                    _referenceData.Add(parameters[param.Name]);
                    continue;
                }

                if (param.Type == ParamVariableType.Int)
                {
                    var value = parameters.GetInt(param.Name).Value;

                    _intData.Add(new IntSerializedParamVariable() { Name = param.Name, Value = value });
                    index = _intData.Count - 1;
                }

                else if (param.Type == ParamVariableType.Float)
                {
                    var value = parameters.GetFloat(param.Name).Value;

                    _floatData.Add(new FloatSerializedParamVariable() { Name = param.Name, Value = value });
                    index = _floatData.Count - 1;
                }

                else
                {
                    var value = parameters.GetBool(param.Name).Value;

                    _boolData.Add(new BoolSerializedParamVaraible() { Name = param.Name, Value = value });
                    index = _boolData.Count - 1;
                }

                _referenceData.Add(new ParamReferenceVariable
                {
                    Name = param.Name,
                    Type = param.Type,
                    Accessibility = param.access,
                    EngineGraph = param.graph,
                    IsPersistent = false,
                    IndexInCollection = index
                });
            }

        }


        #region Update Parameters Methods
        /// <summary>
        /// Update the value of a Int parameter.
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="value">New value</param>
        public void UpdateValue(string name, int value, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Int).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                parameters.UpdateValue(name, value);

            else
                _intData[Ref.IndexInCollection].Value = value;
        }

        /// <summary>
        /// Update the value of a float parameter.
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="value">New value</param>
        public void UpdateValue(string name, float value, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Float).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                parameters.UpdateValue(name, value);

            else
                _floatData[Ref.IndexInCollection].Value = value;
        }

        /// <summary>
        /// Update the value of a bool variable.
        /// </summary>
        /// <param name="name">Name of the variable</param>
        /// <param name="value">New value</param>
        public void UpdateValue(string name, bool value, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Bool).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                parameters.UpdateValue(name, value);

            else
                _boolData[Ref.IndexInCollection].Value = value;
        }

        public void UpdateValue(string name, string value, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.String).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                parameters.UpdateValue(name, value);

            //Rehacer
        }
        #endregion


        #region Get Parameters Methods
        public int GetInt(string name, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Int).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                return parameters.GetInt(Ref.IndexInCollection).Value;

            return _intData[Ref.IndexInCollection].Value;
        }
        public float GetFloat(string name, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Float).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                return parameters.GetFloat(Ref.IndexInCollection).Value;

            return _floatData[Ref.IndexInCollection].Value;
        }
        public bool GetBool(string name, Params parameters)
        {
            var Ref = _referenceData.Where(x => x.Type == ParamVariableType.Bool).Where(x => x.Name == name).First();

            if (Ref.IsPersistent)
                return parameters.GetBool(Ref.IndexInCollection).Value;

            return _boolData[Ref.IndexInCollection].Value;
        }
        #endregion


        #region Log Methods
        public void AddLog(string message) => _textLog.Add(message);
        public void RemoveLog(string message) => _textLog.Remove(message);
        public void RemoveLast() => _textLog.RemoveAt(_textLog.Count);
        public void ClearText() => _textLog.Clear();
        #endregion


        //Agregar el guardado del estado visual de la partida.
    }
}