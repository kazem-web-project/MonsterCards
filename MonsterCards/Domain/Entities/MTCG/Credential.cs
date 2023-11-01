namespace MonsterCards.Domain.Entities.MTCG

{
        [Serializable]
    public class Credential
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

        public override string ToString()
        {
            return this.Username + " :" + this.Password;
        }
    }
}