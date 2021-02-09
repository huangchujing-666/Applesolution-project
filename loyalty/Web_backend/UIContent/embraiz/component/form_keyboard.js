Ext.define('com.embraiz.component.formkeyboard', {
    keyboard: function (form, json_data) {
        if (json_data.isType == true) {
            var form_item = form.items;
            Ext.each(form.items, function (o, j) {
                Ext.each(form.items.get(j).items, function (o, i) {
                    var config = [{
                        key: 37,
                        fn: function (key, e) {
                            if (form.items.get(j).items.get(i).readOnly != true && i % 2 != 0) {
                                if (form.items.get(j).items.get(i).value != '' && form.items.get(j).items.get(i).value != null) {
                                    var len = getPosition(form.items.get(j).items.get(i));
                                    if (len == 0) {
                                        if (form.items.get(j).items.get(i - 1).getXType() == 'checkboxgroup') {
                                            var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i - 1).getId()));
                                            Ext.each(t, function (o, i, t) {
                                                t[0].focus();
                                            });
                                        } else {
                                            form.items.get(j).items.get(i - 1).focus();
                                            e.stopPropagation();
                                            return false;
                                        }
                                    }
                                } else {
                                    if (form.items.get(j).items.get(i - 1).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i - 1).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                    } else {
                                        form.items.get(j).items.get(i - 1).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                        },
                        scope: form.items,
                        stopEvent: false
                    }, {
                        key: 38,
                        fn: function (key, e) {
                            if (j > 0 && i == 0) {
                                if (form.items.get(j - 1).items.getCount() % 2 == 0) {
                                    if (form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 1).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 1).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 1).focus(true);
                                    }
                                } else {
                                    if (form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 2).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 2).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(j - 1).items.get(form.items.get(j - 1).items.getCount() - 2).focus(true);
                                    }
                                }
                                e.stopPropagation();
                                return false;
                            }
                            if (i > 1 && i < form.items.get(j).items.getCount()) {
                                if (form.items.get(j).items.get(i - 2).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i - 2).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                    return false;
                                } else {
                                    form.items.get(j).items.get(i - 2).focus(true);
                                    e.stopPropagation();
                                    return false;
                                }
                            }
                            if (i == 1) {
                                if (form.items.get(j).items.getCount() % 2 == 0) {
                                    if (form.items.get(j).items.get(form.items.get(j).items.getCount() - 2).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(form.items.get(j).items.getCount() - 2).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(j).items.get(form.items.get(j).items.getCount() - 2).focus(true);
                                    }
                                } else {
                                    if (form.items.get(j).items.get(form.items.get(j).items.getCount() - 1).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(form.items.get(j).items.getCount() - 1).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(j).items.get(form.items.get(j).items.getCount() - 1).focus(true);
                                    }
                                }
                                e.stopPropagation();
                                return false;
                            }
                        },
                        scope: form.items,
                        stopEvent: false
                    }, {
                        key: 39,
                        fn: function (key, e) {
                            if (form.items.get(j).items.get(i).readOnly != true) {
                                if (form.items.get(j).items.get(i).value != '' && form.items.get(j).items.get(i).value != null) {
                                    var len = getPosition(form.items.get(j).items.get(i));
                                    if (form.items.get(j).items.get(i).getRawValue().length == len) {
                                        if (form.items.get(j).items.get(i + 1).getXType() == 'checkboxgroup') {
                                            var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i + 1).getId()));
                                            Ext.each(t, function (o, i, t) {
                                                t[0].focus();
                                            });
                                            e.preventDefault();
                                        } else {
                                            form.items.get(j).items.get(i + 1).focus();
                                            e.stopPropagation();
                                            return false;
                                        }
                                    }
                                } else {
                                    if (form.items.get(j).items.get(i + 1).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i + 1).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                    } else {
                                        form.items.get(j).items.get(i + 1).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                        },
                        scope: form.items,
                        stopEvent: false
                    }, {
                        key: 40,
                        fn: function (key, e) {
                            if (i < form.items.get(j).items.getCount() - 2) {
                                if (form.items.get(j).items.get(i + 2).getXType() == 'checkboxgroup') {
                                    var t = Ext.DomQuery.select('input[type=button]', Ext.getDom(form.items.get(j).items.get(i + 2).getId()));
                                    Ext.each(t, function (o, i) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                    return false;
                                } else if (form.items.get(j).items.get(i + 2).getXType() == 'hiddenfield') {
                                    form.items.get(j).items.get(1).focus();
                                    e.stopPropagation();
                                    return false;
                                } else {
                                    form.items.get(j).items.get(i + 2).focus();
                                    e.stopPropagation();
                                    return false;
                                }
                            } else if (i == form.items.get(j).items.getCount() - 1) {
                                if (form.items.get(j).items.getCount() % 2 == 0) {
                                    if (form.items.get(j + 1).items.get(0).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j + 1).items.get(0).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                        return false;
                                    } else {
                                        form.items.get(j + 1).items.get(0).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                } else {
                                    if (form.items.get(j).items.get(1).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(1).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                        return false;
                                    } else {
                                        form.items.get(j).items.get(1).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            } else {
                                if (form.items.get(j).items.getCount() % 2 == 0) {
                                    form.items.get(j).items.get(1).focus();
                                    e.stopPropagation();
                                    return false;
                                } else {
                                    form.items.get(j + 1).items.get(0).focus();
                                    e.stopPropagation();
                                    return false;
                                }
                            }
                        },
                        scope: form.items,
                        stopEvent: false
                    }, {
                        key: 13,
                        fn: function (key, e) {
                            var types = form.items.get(j).items.get(i).getXTypes().split('/');
                            if (types[types.length - 1] == 'combobox' || types[types.length - 1] == 'datefield') {
                                if (i == form.items.get(j).items.getCount() - 2) {
                                    form.items.get(j).items.get(1).focus();
                                } else if (i == form.items.get(j).items.getCount() - 1) {
                                    form.items.get(j + 1).items.get(0).focus();
                                } else if (form.items.get(j).items.get(i + 2).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(j).items.get(i + 2).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                    return false;
                                } else {
                                    form.items.get(j).items.get(i + 2).focus();
                                }
                            } else if (types[types.length - 1] == 'checkboxgroup') {
                                if (form.items.get(j).items.get(i).checked == false) {
                                    form.items.get(j).items.get(i).checked == true;
                                } else {
                                    form.items.get(j).items.get(i).checked == false;
                                }
                            }
                        },
                        scope: form.items,
                        stopEvent: false
                    }];
                    new Ext.util.KeyMap(form.items.get(j).items.get(i).id, config);
                });
            }); ////////////////////////////////////////////////////////////////////////////////////////
        } else {
            Ext.each(form.items.get(0).items.get(0).items, function (o, i) {
                var config = [{
                    key: 38,
                    //上移
                    fn: function (key, e) {
                        if (i > 0 && i < form.items.get(0).items.get(0).items.getCount()) {
                            if (form.items.get(0).items.get(0).items.get(i - 1).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            }
                            if (form.items.get(0).items.get(0).items.get(i - 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(0).items.get(i - 1).focus();
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 40,
                    //下移
                    fn: function (key, e) {
                        if (i < form.items.get(0).items.get(0).items.getCount() - 2) {
                            if (form.items.get(0).items.get(0).items.get(i + 1).getXType() == 'checkboxgroup') {
                                var t = Ext.DomQuery.select('input[type=button]', Ext.getDom(form.items.get(0).items.get(0).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                                return false;
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                                e.stopPropagation();
                                return false;
                            }
                        } else if (i == form.items.get(0).items.get(0).items.getCount() - 2) {
                            if (form.items.get(0).items.get(0).items.get(i + 1).getXType() == 'hiddenfield') {
                                form.items.get(0).items.get(1).items.get(0).focus();
                                e.stopPropagation();
                                return false;
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                            }
                        } else if (i == form.items.get(0).items.get(0).items.getCount() - 1) {
                            if (form.items.get(0).items.get(1).items.get(0).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(0).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                                return false;
                            } else {
                                form.items.get(0).items.get(1).items.get(0).focus();
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 37,
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                            Ext.each(t, function (o, i, t) {
                                t[0].focus();
                            });
                        }
                    }
                }, {
                    key: 39,
                    //右移
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.getCount() >= i) {
                            if (form.items.get(0).items.get(1).items.get(i).readOnly != true) {
                                if (form.items.get(0).items.get(0).items.get(i).value != '' && form.items.get(0).items.get(0).items.get(i).value != null) {
                                    var len = getPosition(form.items.get(0).items.get(0).items.get(i));
                                    if (form.items.get(0).items.get(0).items.get(i).getRawValue().length == len) {
                                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'checkboxgroup') {
                                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                            Ext.each(t, function (o, i, t) {
                                                t[0].focus();
                                            });
                                            e.preventDefault();
                                        } else {
                                            form.items.get(0).items.get(1).items.get(i).focus();
                                            e.stopPropagation();
                                            return false;
                                        }
                                    }
                                } else {
                                    if (form.items.get(0).items.get(1).items.get(i).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                    }
                                    if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                        Ext.each(t, function (o, j, t) {
                                            t[0].focus();
                                        });
                                    } else {
                                        form.items.get(0).items.get(1).items.get(i).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 13,
                    fn: function (key, e) {
                        var types = form.items.get(0).items.get(0).items.get(i).getXTypes().split('/');
                        if (types[types.length - 1] == 'combobox' || types[types.length - 1] == 'datefield') {
                            if (i == form.items.get(0).items.get(0).items.getCount() - 2) {
                                if (form.items.get(0).items.get(0).items.get(i + 1).getXTypes() == 'hiddenfield') {
                                    form.items.get(0).items.get(1).items.get(0).focus();
                                } else {
                                    form.items.get(0).items.get(0).items.get(i + 1).focus();
                                }
                            } else if (i == form.items.get(0).items.get(0).items.getCount() - 1) {
                                form.items.get(0).items.get(1).items.get(0).focus();
                            } else {
                                form.items.get(0).items.get(0).items.get(i + 1).focus();
                            }
                        } else if (types[types.length - 1] == 'checkboxgroup') {
                            if (form.items.get(0).items.get(0).items.get(i).checked == false) {
                                form.items.get(0).items.get(0).items.get(i).checked == true;
                            } else {
                                form.items.get(0).items.get(0).items.get(i).checked == false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }];
                new Ext.util.KeyMap(form.items.get(0).items.get(0).items.get(i).id, config);
            });

            Ext.each(form.items.get(0).items.get(1).items, function (o, i) {
                var config = [{
                    key: 38,
                    //上移
                    fn: function (key, e) {
                        if (i == 0) {
                            if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getXType() == 'hiddenfield') {
                                if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 2).focus();
                                }
                            } else {
                                if (form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(form.items.get(0).items.get(0).items.getCount() - 1).focus();
                                }
                            }
                        }
                        if (i > 0 && i < form.items.get(0).items.get(1).items.getCount()) {
                            if (form.items.get(0).items.get(1).items.get(i - 1).getXType() == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (form.items.get(0).items.get(1).items.get(i - 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i - 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(1).items.get(i - 1).focus(true);
                                e.stopPropagation();
                                return false;
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 40,
                    //下移
                    fn: function (key, e) {
                        if (i < form.items.get(0).items.get(1).items.getCount() - 1) {
                            if (form.items.get(0).items.get(1).items.get(i + 1).getXType() == 'checkboxgroup') {
                                var t = Ext.DomQuery.select('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            }
                            if (form.items.get(0).items.get(1).items.get(i + 1).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus(true);
                            }
                            e.stopPropagation();
                            return false;
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 37,
                    //左移
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).readOnly != true) {
                            if (form.items.get(0).items.get(1).items.get(i).value != '' && form.items.get(0).items.get(1).items.get(i).value != null) {
                                var len = getPosition(form.items.get(0).items.get(1).items.get(i));
                                if (len == 0) {
                                    if (form.items.get(0).items.get(0).items.get(i).getXType() == 'checkboxgroup') {
                                        var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i).getId()));
                                        Ext.each(t, function (o, i, t) {
                                            t[0].focus();
                                        });
                                        e.preventDefault();
                                    } else {
                                        form.items.get(0).items.get(0).items.get(i).focus();
                                        e.stopPropagation();
                                        return false;
                                    }
                                }
                            }
                            if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                                Ext.each(t, function (o, j, t) {
                                    if (document.activeElement.id == t[0].id) {
                                        form.items.get(0).items.get(0).items.get(i).focus();
                                    } else {
                                        t[0].focus();
                                    }
                                    e.stopPropagation();
                                    return false;
                                });
                            } else {
                                if (form.items.get(0).items.get(0).items.get(i).getXType() == 'checkboxgroup') {
                                    var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(0).items.get(i).getId()));
                                    Ext.each(t, function (o, i, t) {
                                        t[0].focus();
                                    });
                                    e.preventDefault();
                                } else {
                                    form.items.get(0).items.get(0).items.get(i).focus();
                                    e.stopPropagation();
                                    return false;
                                }
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }, {
                    key: 39,
                    fn: function (key, e) {
                        if (form.items.get(0).items.get(1).items.get(i).getXType() == 'container') {
                            var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i).getId()));
                            Ext.each(t, function (o, i, t) {
                                t[1].focus();
                                new Ext.KeyMap(t[1].id, {
                                    key: 37,
                                    fn: function (key, e) {
                                        t[0].focus();
                                        e.preventDefault();
                                    }
                                });
                            });
                        }
                    }
                }, {
                    key: 13,
                    fn: function (key, e) {
                        var types = form.items.get(0).items.get(1).items.get(i).getXTypes().split('/');
                        if (types[types.length - 1] == 'combobox') {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (typess[typess.length - 1] == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        } else if (types[types.length - 1] == 'checkboxgroup') {
                            if (form.items.get(0).items.get(1).items.get(i).checked == false) {
                                form.items.get(0).items.get(1).items.get(i).checked == true;
                            } else {
                                form.items.get(0).items.get(1).items.get(i).checked == false;
                            }
                        } else if (types[types.length - 1] == 'datefield') {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        } else {
                            var typess = form.items.get(0).items.get(1).items.get(i + 1).getXTypes().split('/');
                            if (typess[typess.length - 1] == 'checkboxgroup') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else if (typess[typess.length - 1] == 'container') {
                                var t = Ext.query('input', Ext.getDom(form.items.get(0).items.get(1).items.get(i + 1).getId()));
                                Ext.each(t, function (o, i, t) {
                                    t[0].focus();
                                });
                                e.preventDefault();
                            } else {
                                form.items.get(0).items.get(1).items.get(i + 1).focus();
                            }
                        }
                    },
                    scope: form,
                    stopEvent: false
                }];
                new Ext.util.KeyMap(form.items.get(0).items.get(1).items.get(i).id, config);
            });
        }

        function getPosition(obj) {
            var result = 0;
            if (document.all) { //IE  
                var rng;
                if (obj.tagName == "TEXTAREA") { //如果是文本域  
                    rng = event.srcElement.createTextRange();
                    rng.moveToPoint(event.x, event.y);
                } else { //输入框
                    rng = document.selection.createRange();
                }
                rng.moveStart("character", -event.srcElement.value.length);
                result = rng.text.length;
            } else { //非IE浏览器 
                var t = Ext.query('input', Ext.getDom(obj.getId()));
                Ext.each(t, function (o, i, t) {
                    result = t[0].selectionStart;
                });
            }
            return result;
        }
    }
});