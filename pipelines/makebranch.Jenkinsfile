pipeline
{
	agent any
	options
	{
		skipStagesAfterUnstable()
	}
	environment
	{
		/*Parameter: BRANCHNAME, the name of the branch to make */
		GITREPO='dev.azure.com/ASI-ALL/RD-PRODUCT/_git/RO-BackEnd'
		GITCRED="AzureDevOps-Andromeda-CP1"
		EMAIL="devops@amsoftware.com"
		COMMITID=sh(script:'''git rev-parse --short HEAD''',returnStdout:true).trim()
	}
	stages
	{
		stage('Make Branch')
		{
			environment
			{
				GITAUTH = credentials("$GITCRED")
			}
			steps
			{
				sh '''
					git checkout -b "rel/$BRANCHNAME.0" || true
					git push https://$GITAUTH@$GITREPO "rel/$BRANCHNAME.0" || true
				'''
			}
		}
	}
	post
	{
		success
		{
			emailext body: "<h2>rel/$BRANCHNAME.0</h2><p>The branch rel/$BRANCHNAME.0 has been created in repo https://$GITREPO.</p><p>Please make all the necessary adjustments. Git commit: $COMMITID</p><p>Items to consider adjusting:<ul><li>Adding branch to be build on commit</li><li>Setting SETUP script for branch</li><li>Preparing Jenkins pipelines for branch</li></ul></p>", subject: "Branch rel/$BRANCHNAME.0 created", to: "$EMAIL"
			/*office365ConnectorSend status: 'Branch Created', color: '#66e066', message: "The following branch has been created (from git commit $COMMITID)\n\n## rel/$BRANCHNAME.0", webhookUrl: 'https://outlook.office.com/webhook/2da41bbb-b287-4604-880e-25b338d9d906@a760b4ce-2498-4033-92d9-66ad581ec423/IncomingWebhook/2732764141b649c0a96c6711c10e7657/41a7115c-acf6-4441-bb08-118a46cf90d9'*/
		}
	}
}