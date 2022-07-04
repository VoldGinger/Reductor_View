using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class DetailController
{

    public List<Detail> Details => _details;
    public DetailController(Transform mainDetail, float detailSpacing, Material basicMaterial, Material transparentMaterial, float detailMovementSpeed)
    {
        _detailSpacing = detailSpacing;
        _detailMovementSpeed = detailMovementSpeed;
        _details = new List<Detail>();
        _details.AddRange(mainDetail.GetComponentsInChildren<Detail>());
        foreach (var detail in _details)
        {
            detail.Init(basicMaterial, transparentMaterial);
        }
    }
    private readonly List<Detail> _details;
    private readonly float _detailSpacing;
    private readonly float _detailMovementSpeed;

    public Detail GetDetailByName(string name)
    {
        return _details.Find(x => x.name == name);

    }

    public void ExplodeDetails()
    {
        int half = _details.Count / 2;
        var endDetail = _details.Find(x => x.DetailOrder == (_details.Count - 1));
        var currentCoordinate = -half * _detailSpacing;
        endDetail.transform.DOLocalMoveY(currentCoordinate, _detailMovementSpeed);
        foreach (var detail in _details.Where(x => x != endDetail).OrderByDescending(x => x.DetailOrder))
        {
            currentCoordinate += _detailSpacing;
            detail.transform.DOLocalMoveY(currentCoordinate, _detailMovementSpeed);
        }
    }

    public void CloseDetails()
    {
        foreach (var detail in _details)
        {
            detail.transform.DOLocalMoveY(0, _detailMovementSpeed);
        }
    }

    public void ShowAllDetails()
    {
        foreach (var detail in _details)
        {
            detail.ShowDetail();
        }
    }
    public void ShowComponentSolo(Detail shownDetail)
    {
        foreach (var detail in _details)
        {
            detail.HideDetail();
        }
        shownDetail.ShowDetail();
    }



}
