namespace Tmx.Cleo.Core
{
    /// <summary>
    /// A list of supported chain link categories.
    /// </summary>
    /// <remarks>
    /// Specifying the category of a chain link allowes the chain builder to
    /// put the links in apropriate order.
    /// The chain link order is defined as follows:
    /// <list type="number">
    /// <item>
    /// <see cref="ChainLinkCategory.Auditing"/><br/>
    /// Auditing allows you to file every single attempt of entity modification to your logs.
    /// It is executed even before Authorization is done, so denied requests can be recorded.
    /// </item>
    /// <item>
    /// <see cref="ChainLinkCategory.Authorization"/><br/>
    /// In an Authorization chain link you can verify, that the executing user is allowed to
    /// perform the requested action.
    /// </item>
    /// <item>
    /// <see cref="ChainLinkCategory.Validation"/><br/>
    /// After the request is authorized, the modified values can be validated.
    /// </item>
    /// <item>
    /// <see cref="ChainLinkCategory.ChangeTracking"/><br/>
    /// This is another kind of auditing step but will only execute, if preceeding Authorization
    /// and Validation was successful. If either of those steps fail, ChangeTracking won't be executed.
    /// </item>
    /// <item>
    /// <see cref="ChainLinkCategory.Action"/><br/>
    /// The last step in the chain is the Action step. This finally performs the modification and
    /// (if defined that way) writes it back to the data store. Usually there is only one
    /// Action chain link which is the attachment chain link the CLEO exposes.
    /// </item>
    /// </list>
    /// <br/>
    /// A chain can contain multiple chain links of the same category. In this case, basically all of them
    /// will be executed. Of course, if any of the chain links denies the request (for example a Validation chain link),
    /// execution will end at this point and following chain links won't be executed.
    /// </remarks>
    public enum ChainLinkCategory
    {
        /// <summary>
        /// Executed on every request to be able to file any attept to the logs.
        /// </summary>
        Auditing,
        /// <summary>
        /// Perform authorization of the request.
        /// </summary>
        Authorization,
        /// <summary>
        /// Validate the request.
        /// </summary>
        Validation,
        /// <summary>
        /// Executed after all previous steps were passed to file changes to the logs.
        /// </summary>
        ChangeTracking,
        /// <summary>
        /// Perform the action on the CLEO.
        /// </summary>
        Action
    }
}
