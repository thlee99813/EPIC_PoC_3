using System.Collections.Generic;
using UnityEngine;

public class ZombieTickSystem : MonoBehaviour
{
    [SerializeField] private int _detectCountPerFrame = 5;

    private readonly List<ZombieController> _zombies = new();
    private int _detectIndex;

    public void Register(ZombieController zombie)
    {
        if (_zombies.Contains(zombie))
        {
            return;
        }

        _zombies.Add(zombie);
    }

    public void Unregister(ZombieController zombie)
    {
        _zombies.Remove(zombie);

        if (_detectIndex >= _zombies.Count)
        {
            _detectIndex = 0;
        }
    }

    private void Update()
    {
        if (_zombies.Count == 0)
        {
            return;
        }

        int tickCount = Mathf.Min(_detectCountPerFrame, _zombies.Count);

        for (int i = 0; i < tickCount; i++)
        {
            if (_detectIndex >= _zombies.Count)
            {
                _detectIndex = 0;
            }

            ZombieController zombie = _zombies[_detectIndex];
            _detectIndex++;

            if (zombie == null)
            {
                continue;
            }

            zombie.TickDetect();
        }
    }
}
