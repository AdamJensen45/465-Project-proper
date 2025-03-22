using CORE.APP.Domain;

namespace CORE.APP.Features
{
    /// <summary>
    /// Represents a response to a query, inheriting from <see cref="Entity"/>.
    /// </summary>
    public class QueryResponse : Entity
    {
    }

    /// <summary>
    /// Represents a response to a command, inheriting from <see cref="Entity"/>.
    /// </summary>
    public class CommandResponse : Entity
    {
        /// <summary>
        /// Gets a value indicating whether the command was successful.
        /// </summary>
        public bool IsSuccessful { get; }

        /// <summary>
        /// Gets the message associated with the command response.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandResponse"/> class.
        /// </summary>
        /// <param name="isSuccessful">Indicates whether the command was successful.</param>
        /// <param name="message">The message associated with the command response.</param>
        /// <param name="id">The ID associated with the entity.</param>
        public CommandResponse(bool isSuccessful, string message = "", int id = 0) : base(id)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }
    }
}