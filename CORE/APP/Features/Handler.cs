using System.Globalization;

namespace CORE.APP.Features
{
    /// <summary>
    /// Abstract base class for handling operations with culture-specific settings.
    /// </summary>
    public abstract class Handler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Handler"/> class with the specified culture information.
        /// </summary>
        /// <param name="cultureInfo">The culture information to set for the current thread.</param>
        protected Handler(CultureInfo cultureInfo)
        {
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        /// <summary>
        /// Creates a success <see cref="CommandResponse"/> with an optional message and ID.
        /// </summary>
        /// <param name="message">The success message.</param>
        /// <param name="id">The ID associated with the success response.</param>
        /// <returns>A success <see cref="CommandResponse"/>.</returns>
        public CommandResponse Success(string message = "", int id = 0) => new CommandResponse(true, message, id);

        /// <summary>
        /// Creates an error <see cref="CommandResponse"/> with an optional message.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <returns>An error <see cref="CommandResponse"/>.</returns>
        public CommandResponse Error(string message = "") => new CommandResponse(false, message);
    }
}