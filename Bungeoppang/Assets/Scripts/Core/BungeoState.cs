namespace Bungeoppang.Core
{
    public enum BungeoState
    {
        Empty,      // 빈 틀
        Batter,     // 1차 반죽 (밑바닥)
        Filling,    // 소 넣는 중 (팥/슈크림)
        Covering,   // 2차 반죽 (덮기)
        Cooking,    // 익어가는 중
        Perfect,    // 완벽함 (황금빛)
        Burnt       // 타버림 (검은색)
    }

    public enum BungeoFilling
    {
        None,
        RedBean,
        Cream
    }
}
