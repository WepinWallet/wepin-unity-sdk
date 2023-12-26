using System;
public class WepinException : Exception
{
    public WepinException(string error) : base(error) { }
}