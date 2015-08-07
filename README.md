# WebResource Proxy
This is a little hack that helps develop web resources for Microsoft Dynamics CRM - no more updating and publishing. Tested on MS CRM 2015 & 2013 on-premise.
## Usage
First of all you need a webserver that will host webresources that you working on. Next install WebResourceProxy solution from releases or build assembly yourself and fill the description field in the webresource with the next text:

    proxy:http://host:port/some_webresource.htm
    
And now all requests to this webresource will be proxified to http://host:port/some_webresource.htm
