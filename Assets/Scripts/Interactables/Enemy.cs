using System;
using AndoomiUtils;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Interactable
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float speed;
    [SerializeField] float distanceFromCenter;
    [SerializeField] AnimationCurve motionCurve;
    float _curveScale;
    [SerializeField] float curveTime = 1;
    
    [SerializeField] float maxHealth;

    [SerializeField] Image healthBar;
    [SerializeField] Transform worldCanvas;

    float _currentHealth;
    public float Health => _currentHealth;
    public float HealthPercentage => _currentHealth / maxHealth;

    Vector3 _targetPosition;
    public void SetTargetPosition(Vector3 tP) => _targetPosition = tP;

    DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> curveTween;

    Vector3 _originalScale;
    public bool TakeDamage(float damageAmount)
    {
        _currentHealth -= damageAmount;
        healthBar.fillAmount = HealthPercentage;
        if(_currentHealth <= 0)
        {
            if (curveTween != null) curveTween.Kill();
            curveTween = DOTween.To(() => _curveScale, x => _curveScale = x, 0, curveTime);

            curveTween.onComplete += () =>
            {
                Destroy(gameObject);
                Instantiate(coinPrefab, transform.position + Vector3.up*0.5f, coinPrefab.transform.rotation);
            };
            StopInteract(_tempP);
            Destroy(GetComponent<Collider>());
            return true;
        }else
        {
            _curveScale = 0.6f;
            if (curveTween != null) curveTween.Kill();
            curveTween = DOTween.To(() => _curveScale, x => _curveScale = x, 1, curveTime);
        }

        return false;
    }

    private void Start()
    {
        curveTween = DOTween.To(() => _curveScale, x => _curveScale = x, 1, curveTime);

        _currentHealth = maxHealth;
        _originalScale = transform.localScale;
        
        try
        {
            var upgradesCenter = FindFirstObjectByType<UpgradeCenter>();
            Vector3 newTarget = upgradesCenter.transform.position + (transform.position - upgradesCenter.transform.position).normalized * distanceFromCenter;
            SetTargetPosition(newTarget);
        }catch (NullReferenceException)
        {
            Debug.LogWarning("There is no Upgrades Center in this Scene");
        }
    }

    private void Update()
    {
        transform.localScale = _originalScale * motionCurve.Evaluate(_curveScale);

        if (Vector3.Distance(transform.position, _targetPosition) > 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, speed * Time.deltaTime);
        }

        worldCanvas.LookAt(Camera.main.transform.position);
    }

    void GetAttacked()
    {
        TakeDamage(_tempP.Damage);
    }

    Player _tempP;
    public override void Interact(Player p)
    {
        _tempP = p;
        p._attackTimer.OnTimerStart += GetAttacked;
        p._attackTimer.Start();
    }

    public override void StopInteract(Player p)
    {
        p._attackTimer.OnTimerStart -= GetAttacked;
        p._attackTimer.Stop();
        _tempP = null;

    }

    private void OnDestroy()
    {
        if (_tempP != null) StopInteract(_tempP);
    }
}
