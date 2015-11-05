## Post Check-In to Slack
This is a Visual Studio 2013 extension that allows developers to notify their teams of TFS check-ins
through Slack, without having to make any modification to the TFS server. It works entirely through Visual Studio.

You can get started with the plugin by installing Visual Studio 2013 and
the extensions SDK, cloning the repo, building it and running the .vsix file in the
bin folder of the cloned project.

Once it's on the Visual Studio Gallery, it's recommended to download it from there. 

### How it works
The extension adds a section to the Pending Changes view of the Team Explorer.

Choose whether to post the pending check-in to Slack with the Post to Slack? checkbox.
One can then specify:

1. A Slack Webhook URL. Obtain this from the Integrations section
of your Slack settings or ask an administrator of your Slack team.
2. A custom bot name. This can be any name you want. vsbot is a good choice.
3. A custom channel. This needs to be a channel that already exists in your team, and includes the leading
hash. For example: #general

Note: if, for some reason, the extension fails to post to Slack, the check-in will still be made.<br/>
A notification should appear in the Pending Changes view if this scenario occurs.
