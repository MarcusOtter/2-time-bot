using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Logic.Extensions
{
    // A version of https://github.com/dotnet/csharplang/issues/2649#issuecomment-510633766
    // .GetAwaitable replaces .ConfigureAwait(false)
    // Easier to read, no seemingly random boolean value, easy to set project-wide setting (defaults to false here).
    internal static class ConfigureAwaitExtensions
    {
        // IMPORTANT! Defaults to false
        private static bool _continueOnCapturedContext;

        /// <summary>
        /// Returns the given task with .ConfigureAwait(false) by default.
        /// Use <see cref="SetProjectWideConfigureAwaitMode(bool)"/> to change whether or not tasks should continue on captured context for the whole project.
        /// </summary>
        internal static ConfiguredTaskAwaitable GetAwaitable(this Task task)
        {
            return task.ConfigureAwait(_continueOnCapturedContext);
        }

        /// <summary>
        /// Returns the given task with .ConfigureAwait(false) by default.
        /// Use <see cref="SetProjectWideConfigureAwaitMode(bool)"/> to change whether or not tasks should continue on captured context for the whole project.
        /// </summary>
        internal static ConfiguredTaskAwaitable<T> GetAwaitable<T>(this Task<T> task)
        {
            return task.ConfigureAwait(_continueOnCapturedContext);
        }

        /// <summary>
        /// Toggles whether or not tasks should continue on captured context for the whole project (when calling <see cref="GetAwaitable{T}(Task{T})"/>).
        /// </summary>
        internal static void SetProjectWideConfigureAwaitMode(bool continueOnCapturedContext)
        {
            _continueOnCapturedContext = continueOnCapturedContext;
        }
    }
}
