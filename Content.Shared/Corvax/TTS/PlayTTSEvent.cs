using Robust.Shared.Serialization;

namespace Content.Shared.Corvax.TTS;

[Serializable, NetSerializable]
// ReSharper disable once InconsistentNaming
public sealed class PlayTTSEvent : EntityEventArgs
{
    public byte[] Data { get; }
    public NetEntity? SourceUid { get; }
    public bool IsWhisper { get; }
    public bool IsRadio { get; }
    public string? LanguageId { get; } // DS14-Languages

    public PlayTTSEvent(byte[] data, NetEntity? sourceUid = null, bool isWhisper = false, bool isRadio = false, string? languageId = null)
    {
        Data = data;
        SourceUid = sourceUid;
        IsWhisper = isWhisper;
        IsRadio = isRadio;
        LanguageId = languageId; // DS14-Languages
    }
}
