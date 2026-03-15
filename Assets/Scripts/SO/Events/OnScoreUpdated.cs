using CardMatch.Data;
using UnityEngine;

namespace CardMatch.SO.Events
{
    [CreateAssetMenu(menuName = "SO/Events/On Score Updated")]
    public class OnScoreUpdated : GenericEvent<ScoreData>
    {
        
    }
}