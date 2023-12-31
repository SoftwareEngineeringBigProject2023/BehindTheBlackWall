// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: playerService.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace SEServer.Statements.Domain.Shared {

  /// <summary>Holder for reflection information generated from playerService.proto</summary>
  public static partial class PlayerServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for playerService.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PlayerServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNwbGF5ZXJTZXJ2aWNlLnByb3RvEgZwbGF5ZXIaG2dvb2dsZS9wcm90b2J1",
            "Zi9lbXB0eS5wcm90byJeChFFbnRlckdhbWVSZXNwb25zZRIQCghVc2VyTmFt",
            "ZRgBIAEoCRIRCglJbWFnZVBhdGgYAiABKAkSEgoKVG90YWxTY29yZRgDIAEo",
            "AxIQCghUb3RhbEtEQRgEIAEoAyJVChBFbnRlclJvb21SZXF1ZXN0EhQKDEF0",
            "dGFja01vZHVsZRgBIAEoCRIUCgxEZWZlbmRNb2R1bGUYAiABKAkSFQoNUmVj",
            "b3Zlck1vZHVsZRgDIAEoCTKOAQoNUGxheWVyU2VydmljZRI+CglFbnRlckdh",
            "bWUSFi5nb29nbGUucHJvdG9idWYuRW1wdHkaGS5wbGF5ZXIuRW50ZXJHYW1l",
            "UmVzcG9uc2USPQoJRW50ZXJSb29tEhgucGxheWVyLkVudGVyUm9vbVJlcXVl",
            "c3QaFi5nb29nbGUucHJvdG9idWYuRW1wdHlCJKoCIVNFU2VydmVyLlN0YXRl",
            "bWVudHMuRG9tYWluLlNoYXJlZGIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.EmptyReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::SEServer.Statements.Domain.Shared.EnterGameResponse), global::SEServer.Statements.Domain.Shared.EnterGameResponse.Parser, new[]{ "UserName", "ImagePath", "TotalScore", "TotalKDA" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::SEServer.Statements.Domain.Shared.EnterRoomRequest), global::SEServer.Statements.Domain.Shared.EnterRoomRequest.Parser, new[]{ "AttackModule", "DefendModule", "RecoverModule" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class EnterGameResponse : pb::IMessage<EnterGameResponse>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<EnterGameResponse> _parser = new pb::MessageParser<EnterGameResponse>(() => new EnterGameResponse());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EnterGameResponse> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SEServer.Statements.Domain.Shared.PlayerServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterGameResponse() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterGameResponse(EnterGameResponse other) : this() {
      userName_ = other.userName_;
      imagePath_ = other.imagePath_;
      totalScore_ = other.totalScore_;
      totalKDA_ = other.totalKDA_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterGameResponse Clone() {
      return new EnterGameResponse(this);
    }

    /// <summary>Field number for the "UserName" field.</summary>
    public const int UserNameFieldNumber = 1;
    private string userName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string UserName {
      get { return userName_; }
      set {
        userName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "ImagePath" field.</summary>
    public const int ImagePathFieldNumber = 2;
    private string imagePath_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ImagePath {
      get { return imagePath_; }
      set {
        imagePath_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "TotalScore" field.</summary>
    public const int TotalScoreFieldNumber = 3;
    private long totalScore_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long TotalScore {
      get { return totalScore_; }
      set {
        totalScore_ = value;
      }
    }

    /// <summary>Field number for the "TotalKDA" field.</summary>
    public const int TotalKDAFieldNumber = 4;
    private long totalKDA_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public long TotalKDA {
      get { return totalKDA_; }
      set {
        totalKDA_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EnterGameResponse);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EnterGameResponse other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (UserName != other.UserName) return false;
      if (ImagePath != other.ImagePath) return false;
      if (TotalScore != other.TotalScore) return false;
      if (TotalKDA != other.TotalKDA) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (UserName.Length != 0) hash ^= UserName.GetHashCode();
      if (ImagePath.Length != 0) hash ^= ImagePath.GetHashCode();
      if (TotalScore != 0L) hash ^= TotalScore.GetHashCode();
      if (TotalKDA != 0L) hash ^= TotalKDA.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (UserName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(UserName);
      }
      if (ImagePath.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ImagePath);
      }
      if (TotalScore != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(TotalScore);
      }
      if (TotalKDA != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(TotalKDA);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (UserName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(UserName);
      }
      if (ImagePath.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ImagePath);
      }
      if (TotalScore != 0L) {
        output.WriteRawTag(24);
        output.WriteInt64(TotalScore);
      }
      if (TotalKDA != 0L) {
        output.WriteRawTag(32);
        output.WriteInt64(TotalKDA);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (UserName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(UserName);
      }
      if (ImagePath.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ImagePath);
      }
      if (TotalScore != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(TotalScore);
      }
      if (TotalKDA != 0L) {
        size += 1 + pb::CodedOutputStream.ComputeInt64Size(TotalKDA);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EnterGameResponse other) {
      if (other == null) {
        return;
      }
      if (other.UserName.Length != 0) {
        UserName = other.UserName;
      }
      if (other.ImagePath.Length != 0) {
        ImagePath = other.ImagePath;
      }
      if (other.TotalScore != 0L) {
        TotalScore = other.TotalScore;
      }
      if (other.TotalKDA != 0L) {
        TotalKDA = other.TotalKDA;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            UserName = input.ReadString();
            break;
          }
          case 18: {
            ImagePath = input.ReadString();
            break;
          }
          case 24: {
            TotalScore = input.ReadInt64();
            break;
          }
          case 32: {
            TotalKDA = input.ReadInt64();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            UserName = input.ReadString();
            break;
          }
          case 18: {
            ImagePath = input.ReadString();
            break;
          }
          case 24: {
            TotalScore = input.ReadInt64();
            break;
          }
          case 32: {
            TotalKDA = input.ReadInt64();
            break;
          }
        }
      }
    }
    #endif

  }

  public sealed partial class EnterRoomRequest : pb::IMessage<EnterRoomRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<EnterRoomRequest> _parser = new pb::MessageParser<EnterRoomRequest>(() => new EnterRoomRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<EnterRoomRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::SEServer.Statements.Domain.Shared.PlayerServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterRoomRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterRoomRequest(EnterRoomRequest other) : this() {
      attackModule_ = other.attackModule_;
      defendModule_ = other.defendModule_;
      recoverModule_ = other.recoverModule_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public EnterRoomRequest Clone() {
      return new EnterRoomRequest(this);
    }

    /// <summary>Field number for the "AttackModule" field.</summary>
    public const int AttackModuleFieldNumber = 1;
    private string attackModule_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AttackModule {
      get { return attackModule_; }
      set {
        attackModule_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "DefendModule" field.</summary>
    public const int DefendModuleFieldNumber = 2;
    private string defendModule_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string DefendModule {
      get { return defendModule_; }
      set {
        defendModule_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "RecoverModule" field.</summary>
    public const int RecoverModuleFieldNumber = 3;
    private string recoverModule_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string RecoverModule {
      get { return recoverModule_; }
      set {
        recoverModule_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as EnterRoomRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(EnterRoomRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (AttackModule != other.AttackModule) return false;
      if (DefendModule != other.DefendModule) return false;
      if (RecoverModule != other.RecoverModule) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (AttackModule.Length != 0) hash ^= AttackModule.GetHashCode();
      if (DefendModule.Length != 0) hash ^= DefendModule.GetHashCode();
      if (RecoverModule.Length != 0) hash ^= RecoverModule.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (AttackModule.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(AttackModule);
      }
      if (DefendModule.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DefendModule);
      }
      if (RecoverModule.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(RecoverModule);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (AttackModule.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(AttackModule);
      }
      if (DefendModule.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(DefendModule);
      }
      if (RecoverModule.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(RecoverModule);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (AttackModule.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AttackModule);
      }
      if (DefendModule.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(DefendModule);
      }
      if (RecoverModule.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(RecoverModule);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(EnterRoomRequest other) {
      if (other == null) {
        return;
      }
      if (other.AttackModule.Length != 0) {
        AttackModule = other.AttackModule;
      }
      if (other.DefendModule.Length != 0) {
        DefendModule = other.DefendModule;
      }
      if (other.RecoverModule.Length != 0) {
        RecoverModule = other.RecoverModule;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            AttackModule = input.ReadString();
            break;
          }
          case 18: {
            DefendModule = input.ReadString();
            break;
          }
          case 26: {
            RecoverModule = input.ReadString();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            AttackModule = input.ReadString();
            break;
          }
          case 18: {
            DefendModule = input.ReadString();
            break;
          }
          case 26: {
            RecoverModule = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
