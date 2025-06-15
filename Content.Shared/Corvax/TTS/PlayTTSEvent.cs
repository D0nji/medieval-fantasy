using Robust.Shared.Serialization;

namespace Content.Shared.Corvax.TTS;

[Serializable, NetSerializable]
// ReSharper disable once InconsistentNaming
public sealed class PlayTTSEvent : EntityEventArgs
{
    public byte[] Data { get; }
    public byte[]? LexiconData { get; } // DS
    public NetEntity? SourceUid { get; }
    public bool IsWhisper { get; }
    public bool IsRadio { get; }
    public string? LanguageId { get; } // DS

    public PlayTTSEvent(byte[] data, byte[]? lexiconData = null, NetEntity? sourceUid = null, bool isWhisper = false, bool isRadio = false, string? languageId = null)
    {
        Data = data;
        LexiconData = lexiconData ?? Array.Empty<byte>();
        SourceUid = sourceUid;
        IsWhisper = isWhisper;
        IsRadio = isRadio;
        LanguageId = languageId;
    }
}
