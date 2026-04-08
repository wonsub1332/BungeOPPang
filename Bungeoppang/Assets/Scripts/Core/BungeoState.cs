namespace Bungeoppang.Core
{
    public enum BungeoState
    {
        Empty,      // 빈 틀
        Batter,     // 반죽 (방금 부음)
        Filling,    // 소 넣는 중 (팥/슈크림)
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
