using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem.Data
{
    public class DVariablesContainer
    {
        private List<DVariableSO> _variablesSOs = new();
        private Dictionary<int, bool> _variables = new();
        
        public DVariablesContainer()
        {
            var _variablesSOs = Resources.LoadAll<DVariableSO>("DS/Variables");
            foreach (var variable in _variablesSOs)
                _variables.Add(variable.Id, variable.Value);
        }

        public bool Get(int id, out bool variable)
        {
            variable = false;
            if (!_variables.ContainsKey(id))
                return false;

            variable = _variables[id];
            return true;
        }

        public bool Set(int id, bool value = true)
        {
            if (!_variables.ContainsKey(id))
                return false;

            _variables[id] = value;
            return true;
        }
    }
}