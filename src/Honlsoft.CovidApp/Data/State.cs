namespace Honlsoft.CovidApp.Data
{
    public class State
    {
        /// <summary>
        /// An ID for the database.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// The abbreviation of the state.
        /// </summary>
        public string Abbreviation { get; set; }
        
        /// <summary>
        /// The name of the state.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// The population of the state.
        /// </summary>
        public int Population { get; set; }
    }
}