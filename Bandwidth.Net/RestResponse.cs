using System;
using System.IO;
using System.Net;

namespace Bandwidth.Net
{
    public class RestResponse
    {
        protected String responseText;
        protected Boolean error;
        protected String contentType;
        protected String location;
        protected int status;

        protected String firstLink;
        protected String lastLink;
        protected String nextLink;
        protected String previousLink;

        public RestResponse()
        {

        }

        public RestResponse(String text, int status)
        {
            this.responseText = text;
            this.error = (status >= 400);
            this.status = status;
        }

        public static RestResponse CreateRestResponse(HttpWebResponse httpResponse)
        {

            RestResponse restResponse = new RestResponse();

            restResponse.SetStatus((int)httpResponse.StatusCode);

            String httpresponseText = "";


            using (var reader = new System.IO.StreamReader(httpResponse.GetResponseStream()))
            {
                httpresponseText = reader.ReadToEnd();
            }

            if (httpresponseText.Length == 0)
                httpresponseText = "{}";


            // TODO There are several more error conditions that should be handled. 
            if (httpresponseText.Contains("access-denied"))
                restResponse.SetError(true);
            else if (restResponse.GetStatus() >= 400)
                restResponse.SetError(true);

            restResponse.SetResponseText(httpresponseText);

            foreach (var header in httpResponse.Headers["Content-Type"])
            {
                restResponse.SetContentType(header.ToString());
            }


            foreach (var header in httpResponse.Headers["Location"])
            {
                restResponse.SetLocation(header.ToString());
            }

            foreach (var header in httpResponse.Headers["Link"])
            {
                restResponse.ParseLinkHeader(header.ToString());
            }


            return restResponse;
        }

        public void SetStatus(int status)
        {
            this.status = status;
        }

        public void SetError(Boolean error)
        {
            this.error = error;
        }

        public int GetStatus()
        {
            return status;
        }

        public void SetResponseText(String responseText)
        {
            this.responseText = responseText;
        }

        public String GetResponseText()
        {
            return responseText;
        }

        public Boolean IsError()
        {
            return error;
        }

        public void SetContentType(String contentType)
        {
            this.contentType = contentType;
        }

        public Boolean IsJson()
        {
            return (this.contentType.ToLower().Contains("application/json"));
        }

        public String GetLocation()
        {
            return location;
        }

        public void SetLocation(String location)
        {
            this.location = location;
        }

        public String GetContentType()
        {
            return contentType;
        }

        public void ParseLinkHeader(String link)
        {
            String[] links = link.Split(',');
            foreach (String part in links)
            {
                String[] segments = part.Split(';');
                if (segments.Length < 2)
                    continue;
                String linkPart = segments[0].Trim();
                if (!linkPart.StartsWith("<") || !linkPart.EndsWith(">"))
                    continue;
                linkPart = linkPart.Substring(1, linkPart.Length - 1);
                for (int i = 1; i < segments.Length; i++)
                {
                    String[] rel = segments[i].Trim().Split('=');
                    if (rel.Length < 2 || !"rel".Equals(rel[0]))
                        continue;
                    String relValue = rel[1];
                    if (relValue.StartsWith("\"") && relValue.EndsWith("\""))
                        relValue = relValue.Substring(1, relValue.Length - 1);
                    if ("first".Equals(relValue))
                        firstLink = linkPart;
                    else if ("last".Equals(relValue))
                        lastLink = linkPart;
                    else if ("next".Equals(relValue))
                        nextLink = linkPart;
                    else if ("previous".Equals(relValue))
                        previousLink = linkPart;
                }
            }
        }

        public String FirstLink
        {
            get { return firstLink; }
            set{ firstLink = value;}
        }

        public String LastLink
        {
            get { return lastLink; }
            set { lastLink = value; }
        }

        public String NextLink
        {
            get { return nextLink; }
            set { nextLink = value; }
        }

        public String PreviousLink
        {
            get { return previousLink; }
            set { previousLink = value; }
        }

    }
}
