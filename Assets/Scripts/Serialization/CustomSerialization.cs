using Mirror;

public static class CustomSerialization
{
    #region Hit Info

    public static void WriteHitInfo(this NetworkWriter writer, HitInfo hitInfo) {

        writer.Write(hitInfo.damage);
        writer.Write(hitInfo.attackerPos);
        writer.Write(hitInfo.hitPoint);

    }

    public static HitInfo ReadHitInfo(this NetworkReader reader) {

        HitInfo hitInfo = new HitInfo();
        hitInfo.damage = reader.ReadInt();
        hitInfo.attackerPos = reader.ReadVector3();
        hitInfo.hitPoint = reader.ReadVector3();

        return hitInfo;

    }

    #endregion
}
