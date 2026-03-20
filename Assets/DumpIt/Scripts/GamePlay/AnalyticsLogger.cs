using Firebase.Analytics;
using UnityEngine;

public static class AnalyticsLogger
{
    private static bool Ready => FirebaseManager.IsFirebaseReady;

    public static void LogGameStart()
    {
        if (!Ready) return;

        FirebaseAnalytics.LogEvent("game_start");
    }

    public static void LogGameOver(int score, int bestScore)
    {
        if (!Ready) return;

        FirebaseAnalytics.LogEvent(
            "game_over",
            new Parameter("score", score),
            new Parameter("best_score", bestScore)
        );
    }

   
}