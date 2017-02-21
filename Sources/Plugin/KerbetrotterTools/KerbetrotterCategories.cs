﻿using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using KSP.UI.Screens;

namespace KerbetrotterTools
{
    [KSPAddon(KSPAddon.Startup.MainMenu, true)]
    public class KerbatrotterCategories : MonoBehaviour
    {
        //create and the icons
        private Texture2D[] icon_filter;

        private Texture2D icon_filter_lifesupport = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_pods = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_engine = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_fuel = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_payload = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_construction = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_coupling = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_electrical = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_ground = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_utility = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_science = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_thermal = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_aero = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_control = new Texture2D(32, 32, TextureFormat.ARGB32, false);
        private Texture2D icon_filter_communication = new Texture2D(32, 32, TextureFormat.ARGB32, false);

        
        internal bool filter = true;

        //set to false when an icon could not be loaded
        private bool isValid = true;

        //a dictionary storing all the categories of the parts
        private Dictionary<string, PartCategories>[] partCategories;

        //The name of the function filter
        //private string filterName = "Filter by Function";

        //The name of the category for the KPBS filter
        //private string functionFilterName = "Feline Utility Rovers";

        //stores wheter the Community Category Kit is available
        private bool CCKavailable = false;

        //The instance of this filter
        //public static KerbatrotterCategories Instance;

        /// <summary>
        /// When the class awakes it inits all the filters it found for the KerbatrotterTools
        /// </summary>
        private void Awake()
        {
            DontDestroyOnLoad(this);

            //search for Community Category Kit
            int numAssemblies = AssemblyLoader.loadedAssemblies.Count;
            for (int i = 0; i < numAssemblies; i++)
            {
                if (AssemblyLoader.loadedAssemblies[i].name.Equals("CCK"))
                {
                    CCKavailable = true;
                    break;
                }
            }

            //if the configuration is null
            if (KerbetrotterConfiguration.Instance() == null)
            {
                Debug.Log("[KerbetrotterTools] ERROR Configuration Instance is null");
                return;
            }


            //get the filterSetings for the kerbetrotter tools
            KerbetrotterFilterSettings[] filterSettings = KerbetrotterConfiguration.Instance().FilterSettings;
            int numFilter = filterSettings.Length;


            partCategories = new Dictionary<string, PartCategories>[numFilter];
            for (int i = 0; i < numFilter; i++)
            {
                //add all the parts that should be in the list
                List<AvailablePart> all_parts = new List<AvailablePart>();

                all_parts.AddRange(PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith(filterSettings[i].IncludeFilter)));

                //remove the parts that are excluded from the filter
                if (!string.IsNullOrEmpty(filterSettings[i].ExcludeFilter))
                {
                    all_parts.RemoveAll(ap => ap.name.StartsWith(filterSettings[i].ExcludeFilter));
                }
                //save all the categories from the parts of this mod
                int numParts = all_parts.Count;
                partCategories[i] = new Dictionary<string, PartCategories>(numParts);
                for (int k = 0; k < numParts; k++)
                {
                    partCategories[i].Add(all_parts[k].name, all_parts[k].category);
                }
            }

            icon_filter = new Texture2D[numFilter];

            //load the icons for the categories
            try
            {
                for (int i = 0; i < numFilter; i++)
                {
                    icon_filter[i] = new Texture2D(32, 32, TextureFormat.ARGB32, false);
                    if (!icon_filter[i].LoadImage(File.ReadAllBytes(filterSettings[i].FilterIcon)))
                    {
                        Debug.Log("[KerbetrotterTools] ERROR loading filter_icon for: " + filterSettings[i].ModName);
                    }
                }

                if (!icon_filter_pods.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_pods.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_pods");
                    isValid = false;
                }
                if (!icon_filter_aero.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_aero.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_aero");
                    isValid = false;
                }
                if (!icon_filter_control.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_control.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_control");
                    isValid = false;
                }
                if (!icon_filter_communication.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_communication.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_communication");
                    isValid = false;
                }
                if (!icon_filter_fuel.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_fueltank.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_fueltank");
                    isValid = false;
                }
                if (!icon_filter_electrical.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_electrical.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_electrical");
                    isValid = false;
                }
                if (!icon_filter_thermal.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_thermal.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_thermal");
                    isValid = false;
                }
                if (!icon_filter_science.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_science.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_science");
                    isValid = false;
                }
                if (!icon_filter_engine.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_engine.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_engine");
                    isValid = false;
                }
                if (!icon_filter_ground.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_ground.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_ground");
                    isValid = false;
                }
                if (!icon_filter_coupling.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_coupling.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_coupling");
                    isValid = false;
                }
                if (!icon_filter_payload.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_payload.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_payload");
                    isValid = false;
                }
                if (!icon_filter_construction.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_construction.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_construction");
                    isValid = false;
                }
                if (!icon_filter_utility.LoadImage(File.ReadAllBytes("GameData/KerbetrotterLtd/000_KerbetrotterTools/Icons/filter_utility.png")))
                {
                    Debug.Log("[KerbetrotterTools] ERROR loading filter_utility");
                    isValid = false;
                }
            }
            catch (Exception e)
            {
                Debug.Log("[KerbetrotterTools] ERROR EXC loading Images" + e.Message);
                isValid = false;
            }

            //Add the Kerbetrotterfilter to the list of filters
            GameEvents.onGUIEditorToolbarReady.Add(KerbetrotterFilter);
            //GameEvents.OnGameSettingsApplied.Add(updateFilterSettings);
        }

        /// <summary>
        /// Removes all listeners from the GameEvents when Class is destroyed
        /// </summary>
        protected void OnDestroy()
        {
            GameEvents.onGUIEditorToolbarReady.Remove(KerbetrotterFilter);
            //GameEvents.OnGameSettingsApplied.Remove(updateFilterSettings);
        }

        /*/// <summary>
        /// Update the settings from the filters
        /// </summary>
        public void updateFilterSettings()
        {
            Debug.Log("[KerbetrotterTools] updateFilterSettings");
            GameEvents.onGUIEditorToolbarReady.Remove(KerbetrotterFunctionFilter);

            if (HighLogic.CurrentGame != null)
            {
                RemoveFunctionFilter();
                AddPartCategories();

                if (HighLogic.CurrentGame.Parameters.CustomParams<KerbetrotterSettings>().groupParts)
                {
                    RemovePartCategories();
                    GameEvents.onGUIEditorToolbarReady.Add(KerbetrotterFunctionFilter);
                }
            }
        }*/


        /// <summary>
        /// Filters parts by their names
        /// </summary>
        /// <param name="part">the part which has to be filtered</param>
        /// <returns></returns>
        private bool filterPart(AvailablePart part, int index)
        {
            KerbetrotterFilterSettings filterSettings = KerbetrotterConfiguration.Instance().FilterSettings[index];
            return part.name.StartsWith(filterSettings.IncludeFilter) && (string.IsNullOrEmpty(filterSettings.ExcludeFilter) || !part.name.StartsWith(filterSettings.ExcludeFilter));
        }

        /*/// <summary>
        /// Remove the fuction filte category
        /// </summary>
        private void RemoveFunctionFilter()
        {
            if (PartCategorizer.Instance != null)
            {
                PartCategorizer.Category Filter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == filterName);
                if (Filter != null)
                {
                    PartCategorizer.Category subFilter = Filter.subcategories.Find(f => f.button.categoryName == functionFilterName);
                    if (subFilter != null)
                    {
                        subFilter.DeleteSubcategory();
                    }
                }
            }
        }

        /// <summary>
        /// Remove the fuction filte category
        /// </summary>
        private void AddFunctionFilter()
        {
            if (PartCategorizer.Instance != null)
            {
                PartCategorizer.Category Filter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == filterName);
                if (Filter != null)
                {
                    PartCategorizer.Category subFilter = Filter.subcategories.Find(f => f.button.categoryName == functionFilterName);
                    if (subFilter != null)
                    {
                        subFilter.DeleteSubcategory();
                    }
                }
            }
        }

        /// <summary>
        /// Add the stored categories to all the parts of KPBS
        /// </summary>
        private void AddPartCategories()
        {
            if (partCategories != null)
            {
                List<AvailablePart> parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith("KKAOSS"));
                for (int i = 0; i < parts.Count; i++)
                {
                    if (partCategories.ContainsKey(parts[i].name))
                    {
                        parts[i].category = partCategories[parts[i].name];
                    }
                }
            }
        }

        /// <summary>
        /// Remove the categories from all parts of KPBS
        /// </summary>
        private void RemovePartCategories()
        {
            List<AvailablePart> parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith("KKAOSS"));
            for (int i = 0; i < parts.Count; i++)
            {
                parts[i].category = PartCategories.none;
            }
        }*/

        /**
         * Filter the parts by their manufacturer
         * 
         * @param[in] part : the part to test
         * @param[in] category : the category of the part
         * 
         * @return[bool] true when categories match, else false
         */
        private bool filterCategories(AvailablePart part, PartCategories category, int index)
        {
            if (index >= KerbetrotterConfiguration.Instance().FilterSettings.Length)
            {
                Debug.LogError("[KerbetrotterTools] invalid index for category filter: " + index);
                return false;
            }

            KerbetrotterFilterSettings filterSettings = KerbetrotterConfiguration.Instance().FilterSettings[index];
            //return false when the part is not included by the filter
            if (!part.name.StartsWith(filterSettings.IncludeFilter) || (!string.IsNullOrEmpty(filterSettings.ExcludeFilter) && part.name.StartsWith(filterSettings.ExcludeFilter)))
            {
                return false;
            }
            return partCategories[index][part.name] == category;
        }

        /*/// <summary>
        /// Add the function filter to the filter
        /// </summary>
        private void KerbetrotterFunctionFilter()
        {
            if (!isValid)
            {
                Debug.LogError("[KerbetrotterTools] invalid");
                return;
            }

            RUI.Icons.Selectable.Icon filterIconSurfaceStructures = new RUI.Icons.Selectable.Icon("KKAOSS_icon_lifeSupport", icon_surface_structures, icon_surface_structures, true);

            if (filterIconSurfaceStructures == null)
            {
                Debug.LogError("[KerbetrotterTools] ERROR filterIconSurfaceStructures cannot be loaded");
                return;
            }

            //Find the function filter
            PartCategorizer.Category functionFilter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == filterName);

            //Add a new subcategory to the function filter
            PartCategorizer.AddCustomSubcategoryFilter(functionFilter, functionFilterName, filterIconSurfaceStructures, p => filter_KKAOSS(p));
        }*/


        /**
         * The function to add the modules of this mod to a separate category 
         */
        private void KerbetrotterFilter()
        {
            if (!isValid)
            {
                Debug.Log("[KerbetrotterTools] invalid");
                return;
            }

            //if the configuration is null
            if (KerbetrotterConfiguration.Instance() == null)
            {
                Debug.Log("[] ERROR Configuration Instance is null");
                return;
            }

            //get the filterSetings for the kerbetrotter tools
            KerbetrotterFilterSettings[] filterSettings = KerbetrotterConfiguration.Instance().FilterSettings;

            //icons for KPSS's own category
            RUI.Icons.Selectable.Icon ic_pods = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_pods", icon_filter_pods, icon_filter_pods, true);
            RUI.Icons.Selectable.Icon ic_aero = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_aero", icon_filter_pods, icon_filter_pods, true);
            RUI.Icons.Selectable.Icon ic_control = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_control", icon_filter_pods, icon_filter_pods, true);
            RUI.Icons.Selectable.Icon ic_communication = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_comm", icon_filter_pods, icon_filter_pods, true);
            RUI.Icons.Selectable.Icon ic_fuels = new RUI.Icons.Selectable.Icon("Kerbetrotter__filter_fuel", icon_filter_fuel, icon_filter_fuel, true);
            RUI.Icons.Selectable.Icon ic_engine = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_engine", icon_filter_engine, icon_filter_engine, true);
            RUI.Icons.Selectable.Icon ic_structural = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_structural", icon_filter_construction, icon_filter_construction, true);
            RUI.Icons.Selectable.Icon ic_payload = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_payload", icon_filter_payload, icon_filter_payload, true);
            RUI.Icons.Selectable.Icon ic_utility = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_utility", icon_filter_utility, icon_filter_utility, true);
            RUI.Icons.Selectable.Icon ic_coupling = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_coupling", icon_filter_coupling, icon_filter_coupling, true);
            RUI.Icons.Selectable.Icon ic_ground = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_ground", icon_filter_ground, icon_filter_ground, true);
            RUI.Icons.Selectable.Icon ic_thermal = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_thermal", icon_filter_thermal, icon_filter_thermal, true);
            RUI.Icons.Selectable.Icon ic_electrical = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_electrical", icon_filter_electrical, icon_filter_electrical, true);
            RUI.Icons.Selectable.Icon ic_science = new RUI.Icons.Selectable.Icon("Kerbetrotter_filter_fuel", icon_filter_science, icon_filter_science, true);
            RUI.Icons.Selectable.Icon ic_lifeSupport = new RUI.Icons.Selectable.Icon("Kerbetrotter_icon_life_support", icon_filter_lifesupport, icon_filter_lifesupport, true);


            int numFilter = filterSettings.Length;
            for (int i = 0; i < numFilter; i++)
            {
                //-----------------own category-----------------

                if (filterSettings[i].ShowModFilter)
                {
                    List<PartCategories> availableCategories = new List<PartCategories>(15);
                    availableCategories.Add(PartCategories.Aero);
                    availableCategories.Add(PartCategories.Communication);
                    availableCategories.Add(PartCategories.Control);
                    availableCategories.Add(PartCategories.Coupling);
                    availableCategories.Add(PartCategories.Electrical);
                    //availableCategories.Add(PartCategories.Engine);//NEEDED?
                    availableCategories.Add(PartCategories.FuelTank);
                    availableCategories.Add(PartCategories.Ground);
                    //leftCategories.Add(PartCategories.none);
                    availableCategories.Add(PartCategories.Payload);
                    availableCategories.Add(PartCategories.Pods);
                    availableCategories.Add(PartCategories.Propulsion);
                    availableCategories.Add(PartCategories.Science);
                    availableCategories.Add(PartCategories.Structural);
                    availableCategories.Add(PartCategories.Thermal);
                    availableCategories.Add(PartCategories.Utility);

                    List<PartCategories> usedCategories = new List<PartCategories>();

                    int numParts = partCategories[i].Count;
                    foreach (KeyValuePair<string, PartCategories> entry in partCategories[i])
                    {
                        if (availableCategories.Contains(entry.Value))
                        {
                            usedCategories.Add(entry.Value);
                            availableCategories.Remove(entry.Value);
                        }
                    }

                    if (usedCategories.Count > 0)
                    {
                        //create the icon for the filter
                        RUI.Icons.Selectable.Icon filterIcon = new RUI.Icons.Selectable.Icon(filterSettings[i].ModName + "_icon_mod", icon_filter[i], icon_filter[i], true);

                        //add the mod to the categories to the categories
                        Color color = filterSettings[i].Color;
                        PartCategorizer.Category modFilter =PartCategorizer.AddCustomFilter(filterSettings[i].ModName, filterIcon, new Color(color.r, color.g, color.b));

                        int index = i;

                        //add subcategories to the KPSS category you just added
                        if (usedCategories.Contains(PartCategories.Pods))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Pods", ic_pods, p => filterCategories(p, PartCategories.Pods, index));
                        }

                        if (usedCategories.Contains(PartCategories.FuelTank))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Fuel Tank", ic_fuels, p => filterCategories(p, PartCategories.FuelTank, index));
                        }

                        if (usedCategories.Contains(PartCategories.Propulsion))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Engines", ic_engine, p => filterCategories(p, PartCategories.Propulsion, index));
                        }

                        if (usedCategories.Contains(PartCategories.Control))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Control", ic_control, p => filterCategories(p, PartCategories.Control, index));
                        }

                        if (usedCategories.Contains(PartCategories.Structural))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Structural", ic_structural, p => filterCategories(p, PartCategories.Structural, index));
                        }

                        if (usedCategories.Contains(PartCategories.Coupling))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Coupling", ic_coupling, p => filterCategories(p, PartCategories.Coupling, index));
                        }

                        if (usedCategories.Contains(PartCategories.Payload))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Payload", ic_payload, p => filterCategories(p, PartCategories.Payload, index));
                        }

                        if (usedCategories.Contains(PartCategories.Aero))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Aerodynamics", ic_aero, p => filterCategories(p, PartCategories.Aero, index));
                        }

                        if (usedCategories.Contains(PartCategories.Ground))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Ground", ic_ground, p => filterCategories(p, PartCategories.Ground, index));
                        }

                        if (usedCategories.Contains(PartCategories.Thermal))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Thermal", ic_thermal, p => filterCategories(p, PartCategories.Thermal, index));
                        }

                        if (usedCategories.Contains(PartCategories.Electrical))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Electrical", ic_electrical, p => filterCategories(p, PartCategories.Electrical, index));
                        }

                        if (usedCategories.Contains(PartCategories.Communication))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Communication", ic_communication, p => filterCategories(p, PartCategories.Communication, index));
                        }

                        if (usedCategories.Contains(PartCategories.Science))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Science", ic_science, p => filterCategories(p, PartCategories.Science, index));
                        }

                        if (usedCategories.Contains(PartCategories.Utility))
                        {
                            PartCategorizer.AddCustomSubcategoryFilter(modFilter, "Utility", ic_utility, p => (filterCategories(p, PartCategories.Utility, index)));
                        }
                    }
                }
                //-----------------end own category-----------------

                //------------subcategory in function filter---------
                if (filterSettings [i].ShowFunctionFilter && (!CCKavailable || !filterSettings[i].DisableForCCK)) 
                {
                    RUI.Icons.Selectable.Icon filterIconSurfaceStructures = new RUI.Icons.Selectable.Icon("Kerbetrotter_function filter", icon_filter[i], icon_filter[i], true);

                    if (filterIconSurfaceStructures == null)
                    {
                        Debug.Log("[KerbetrotterTools] ERROR FilterIcon cannot be loaded for: " + filterSettings[i].ModName);
                        return;
                    }

                    //Find the function filter
                    PartCategorizer.Category functionFilter = PartCategorizer.Instance.filters.Find(f => f.button.categoryName == "Filter by Function");

                    //Add a new subcategory to the function filter
                    PartCategorizer.AddCustomSubcategoryFilter(functionFilter, filterSettings[i].ModName, filterIconSurfaceStructures, p => filterPart(p,i));

                    //Remove the parts from all other categories
                    List<AvailablePart> parts = PartLoader.Instance.loadedParts.FindAll(ap => ap.name.StartsWith(filterSettings[i].IncludeFilter));
                    parts.RemoveAll(ap => ap.name.StartsWith(filterSettings[i].ExcludeFilter));

                    for (int j = 0; j < parts.Count; i++)
                    {
                        parts[j].category = PartCategories.none;
                    }
                }
                //---------end subcategory in function filter-------
            }
        }
    }
}