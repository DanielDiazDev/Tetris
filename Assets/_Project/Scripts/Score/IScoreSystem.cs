namespace _Project.Scripts.Score
{
    public interface IScoreSystem
    {
        int GetScore();
        void AddScore(int amount);
    }
}