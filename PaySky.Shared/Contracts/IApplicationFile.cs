namespace PaySky.Shared.Contracts;

public interface IApplicationFile
{
    string FileName { get; }
    string ContentType { get; }
    long Length { get; }
    Stream OpenReadStream();
}