public interface IHitable
{
    public void OnHit(HitInfo hitInfo);

    public void RPC_ClientOnHit(HitInfo hitInfo);
}
