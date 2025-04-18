using Elsa.Workflows.Helpers;
using Elsa.Workflows.Runtime.Results;

namespace Elsa.Workflows.Runtime;

/// <summary>
/// Contains extension methods for the <see cref="IStimulusSender"/> interface.
/// </summary>
public static class WorkflowBrokerExtensions
{
    /// <summary>
    /// Delivers a stimulus to an activity. This could result in new workflow instances as well as existing workflow instances being resumed.
    /// </summary>
    [Obsolete("Use SendAsync(string name, object stimulus, StimulusMetadata? metadata = null, CancellationToken cancellationToken = default) instead.")]
    public static Task<SendStimulusResult> SendAsync<TActivity>(this IStimulusSender stimulusSender, object stimulus, StimulusMetadata? metadata = null, CancellationToken cancellationToken = default) where TActivity : IActivity
    {
        var name = ActivityTypeNameHelper.GenerateTypeName<TActivity>();
        return stimulusSender.SendAsync(name, stimulus, metadata, cancellationToken);
    }
}