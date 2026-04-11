public class GameInteractor
{
    private readonly GameRepository _repository;

    public GameInteractor(GameRepository repository)
    {
        _repository = repository;
    }

    public void SaveGame(PlayerData data)
    {
        
        if (data == null) return;
        
        _repository.Save(data);
        
    }

    public PlayerData LoadGame()
    {
        PlayerData loaded = _repository.LoadGame();  
        return loaded;        // ← просто возвращаем, без внутренних полей!
    }
}