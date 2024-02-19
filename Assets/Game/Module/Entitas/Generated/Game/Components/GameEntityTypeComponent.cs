//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentEntityApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public partial class GameEntity {

    public Game.Entitas.EntityTypeComponent entityType { get { return (Game.Entitas.EntityTypeComponent)GetComponent(GameComponentsLookup.EntityType); } }
    public bool hasEntityType { get { return HasComponent(GameComponentsLookup.EntityType); } }

    public void AddEntityType(Game.Entitas.EEntityType newM_EntityType) {
        var index = GameComponentsLookup.EntityType;
        var component = (Game.Entitas.EntityTypeComponent)CreateComponent(index, typeof(Game.Entitas.EntityTypeComponent));
        component.m_EntityType = newM_EntityType;
        AddComponent(index, component);
    }

    public void ReplaceEntityType(Game.Entitas.EEntityType newM_EntityType) {
        var index = GameComponentsLookup.EntityType;
        var component = (Game.Entitas.EntityTypeComponent)CreateComponent(index, typeof(Game.Entitas.EntityTypeComponent));
        component.m_EntityType = newM_EntityType;
        ReplaceComponent(index, component);
    }

    public void RemoveEntityType() {
        RemoveComponent(GameComponentsLookup.EntityType);
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by Entitas.CodeGeneration.Plugins.ComponentMatcherApiGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
public sealed partial class GameMatcher {

    static Entitas.IMatcher<GameEntity> _matcherEntityType;

    public static Entitas.IMatcher<GameEntity> EntityType {
        get {
            if (_matcherEntityType == null) {
                var matcher = (Entitas.Matcher<GameEntity>)Entitas.Matcher<GameEntity>.AllOf(GameComponentsLookup.EntityType);
                matcher.componentNames = GameComponentsLookup.componentNames;
                _matcherEntityType = matcher;
            }

            return _matcherEntityType;
        }
    }
}