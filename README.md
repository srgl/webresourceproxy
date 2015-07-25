# WebResource Proxy
This is a little hack that helps develop web resources for Microsoft Dynamics CRM - no more updating and publishing. Tested on MS CRM 2015 on-premise.
## Usage
First of all you need a webserver that will host webresources that you working on. Next install WebResourceProxy solution and fill the description field in the webresource with the next text:
`proxy:http://some_host:31337/some_webresource.htm`
