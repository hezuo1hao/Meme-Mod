using System.Collections.Generic;
using LBoL.Base;
using LBoL.ConfigData;
using LBoLEntitySideloader;
using lvalonmeme.Enemies.Template;


namespace lvalonmeme.Enemies
{
    public sealed class SampleCharacterEnemyGroupDef : lvalonmemeenemygrouptemplate
    {
        public override IdContainer GetId() => nameof(lvalonmeme);

        public override EnemyGroupConfig MakeConfig()
        {
            EnemyGroupConfig config = GetEnemyGroupDefaultConfig();
            config.Name = nameof(lvalonmeme);
            config.FormationName = VanillaFormations.Single;
            config.Enemies = [nameof(lvalonmeme)];
            config.EnemyType = EnemyType.Boss;
            config.RollBossExhibit = true;

            return config;
        }
    }
}