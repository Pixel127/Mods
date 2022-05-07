using RimWorld;
using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace QualityBuilder
{
    class QualityBuilderModSettings
    {
        bool defaultUseQualityBuilderInternal = true;
        int skillDifferenceFromBestBuilderInternal = 0;
        int ignoreQualityBuilderAtSkillInternal = 20;
        QualityCategory defaultMinQualitySettingInternal = QualityCategory.Awful;
        Pawn bestConstructorOverrideInternal;

        public QualityBuilderModSettings()
        { }

        public QualityBuilderModSettings(QualityBuilderModSettings clone)
        {
            if (clone == null)
                clone = fallBack;
            this.bestConstructorOverride = clone.bestConstructorOverride;
            this.skillDifferenceFromBestBuilder = clone.skillDifferenceFromBestBuilder;
            this.ignoreQualityBuilderAtSkill = clone.ignoreQualityBuilderAtSkill;
            this.defaultUseQualityBuilder = clone.defaultUseQualityBuilder;
            this.defaultMinQualitySetting = clone.defaultMinQualitySetting;
            this.bestConstructorOverride = clone.bestConstructorOverride;
        }

        public Pawn bestConstructorOverride
        {
            get { return bestConstructorOverrideInternal; }
            set { bestConstructorOverrideInternal = value; }
        }

        public bool defaultUseQualityBuilder
        {
            get { return defaultUseQualityBuilderInternal; }
            set { defaultUseQualityBuilderInternal = value; }
        }

        public int skillDifferenceFromBestBuilder
        {
            get { return skillDifferenceFromBestBuilderInternal; }
            set { skillDifferenceFromBestBuilderInternal = value; }
        }

        public int ignoreQualityBuilderAtSkill
        {
            get { return ignoreQualityBuilderAtSkillInternal; }
            set { ignoreQualityBuilderAtSkillInternal = value; }
        }

        public QualityCategory defaultMinQualitySetting
        {
            get { return defaultMinQualitySettingInternal; }
            set { defaultMinQualitySettingInternal = value; }
        }

        public void ExposeData()
        {
            Scribe_Values.Look<bool>(ref this.defaultUseQualityBuilderInternal, "defaultUseQBuilder", true);
            Scribe_Values.Look<int>(ref this.skillDifferenceFromBestBuilderInternal, "qBuilderSkillDiff", 0);
            Scribe_Values.Look<int>(ref this.ignoreQualityBuilderAtSkillInternal, "qBuilderIgnoAtSkill", 20);
            Scribe_Values.Look<QualityCategory>(ref this.defaultMinQualitySettingInternal, "desiredMinQuality", QualityCategory.Awful, false);
            Scribe_References.Look<Pawn>(ref this.bestConstructorOverrideInternal, "bestConstructorOverride");
        }

        public void resetToDefault()
        {
            this.defaultUseQualityBuilderInternal = true;
            this.skillDifferenceFromBestBuilderInternal = 0;
            this.ignoreQualityBuilderAtSkillInternal = 20;
            this.defaultMinQualitySettingInternal = QualityCategory.Awful;
            this.bestConstructorOverride = null;
        }

        private static QualityBuilderModSettings getSettings(Map map)
        {
            QualityBuilderModSettings settings = null;
            if (map != null)
            {
                QualityBuilder_MapComponent mapComp = QualityBuilder_MapComponent.getAndEnsure(map);
                //if (mapComp != null && mapComp.useMapSettings)
                if (mapComp != null)
                    settings = mapComp.settings;
            }
            if (settings == null)
                settings = QualityBuilderGlobalModSettings.getSettings();
            if (settings == null)
                settings = QualityBuilderModSettings.fallBack;
            return settings;
        }

        public static bool getDefaultUseQualityBuilder(Map map)
        {
            return getSettings(map).defaultUseQualityBuilderInternal;
        }

        public static int getSkillDifferenceFromBestBuilder(Map map)
        {
            return getSettings(map).skillDifferenceFromBestBuilderInternal;
        }

        public static int getIgnoreQualityBuilderAtSkill(Map map)
        {
            return getSettings(map).ignoreQualityBuilderAtSkillInternal;
        }

        public static QualityCategory getDefaultMinQualitySetting(Map map)
        {
            return getSettings(map).defaultMinQualitySettingInternal;
        }

        public static Pawn getBestConstructorOverride(Map map)
        {
            return getSettings(map).bestConstructorOverrideInternal;
        }

        private static QualityBuilderModSettings fallBack = new QualityBuilderModSettings();
    }
}
