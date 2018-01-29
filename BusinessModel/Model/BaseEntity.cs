using System;

namespace BusinessModel
{
    ///<summary>
    ///Base entity for all class which need
    ///to save in database 
    ///</summary>
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
