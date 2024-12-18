[System.Serializable]
public class GameData
{
    public int levelIndex;       // Aktif sahne numaras�
    public float playerPosX;     // Oyuncunun X pozisyonu
    public float playerPosY;     // Oyuncunun Y pozisyonu
    public float playerHealth;   // Oyuncunun can� (varsa)
    public float enemyPosX;
    public float enemyPosY;
    public float enemyHealth;
    public int coin;
    public int waveCount;
    public int healthUpgradeCount;
    public int damageUpgradeCount;
    public int speedUpgradeCount;
    public int dashSpeedUpgradeCount;
    
}