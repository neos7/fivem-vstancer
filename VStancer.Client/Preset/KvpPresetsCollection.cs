﻿using System;
using System.Collections.Generic;

using static CitizenFX.Core.Native.API;
using Newtonsoft.Json;

namespace VStancer.Client.Preset
{
    /// <summary>
    /// The vstancer preset manager which saves the presets as key-value pairs built-in FiveM
    /// </summary>
    public class KvpPresetsCollection : IPresetsCollection<string, VStancerPreset>
    {
        private string mKvpPrefix;

        public event EventHandler PresetsCollectionChanged;

        public KvpPresetsCollection(string prefix)
        {
            mKvpPrefix = prefix;
        }

        public bool Delete(string name)
        {
            // Check if the preset ID is valid
            if (string.IsNullOrEmpty(name))
                return false;

            // Get the KVP key
            string key = string.Concat(mKvpPrefix, name);

            // Check if a KVP with the given key exists
            if (GetResourceKvpString(key) == null)
                return false;

            // Delete the KVP
            DeleteResourceKvp(key);

            // Invoke the event
            PresetsCollectionChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public bool Save(string name, VStancerPreset preset)
        {
            // Check if the preset and the ID are valid
            if (string.IsNullOrEmpty(name) || preset == null)
                return false;

            // Get the KVP key
            string key = string.Concat(mKvpPrefix, name);

            // Be sure the key isn't already used
            if (GetResourceKvpString(key) != null)
                return false;

            // Get the Json
            var json = JsonConvert.SerializeObject(preset);

            // Save the KVP
            SetResourceKvp(key, json);

            // Invoke the event
            PresetsCollectionChanged?.Invoke(this, EventArgs.Empty);

            return true;
        }

        public VStancerPreset Load(string name)
        {
            // Check if the preset ID is valid
            if (string.IsNullOrEmpty(name))
                return null;

            // Get the KVP key
            string key = string.Concat(mKvpPrefix, name);

            // Get the KVP value
            string value = GetResourceKvpString(key);

            // Check if the value is valid
            if (string.IsNullOrEmpty(value))
                return null;

            // Create a preset
            return JsonConvert.DeserializeObject<VStancerPreset>(value);
        }

        public IEnumerable<string> GetKeys()
        {
            return VStancerUtilities.GetKeyValuePairs(mKvpPrefix);
        }
    }
}