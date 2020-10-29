var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/home";
        });
        $('#btnUpdate-cart').off('click').on('click', function () {
            var listApparatus = $('.txtApparatusQuantity');
            var cartApparatusList = [];
            $.each(listApparatus, function (i, item) {
                cartApparatusList.push({
                    Quantity: $(item).val(),
                    ScientificApparatus: {
                        ScientificApparatusID: $(item).data('id')
                    }
                });
            });
            var listMedicine = $('.txtMedicineQuantity');
            var cartMedicineList = [];
            $.each(listMedicine, function (i, item) {
                cartMedicineList.push({
                    Quantity: $(item).val(),
                    Medicine: {
                        MedicineID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { medicineModel: JSON.stringify(cartMedicineList), apparatusModel: JSON.stringify(cartApparatusList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart";
                    }
                }
            })
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart";
                    }
                }
            })
        });

        $('.btnDelete-medicine').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/DeleteMedicine',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart";
                    }
                }
            })
        });

        $('.btnDelete-apparatus').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: { id: $(this).data('id') },
                url: '/Cart/DeleteApparatus',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart";
                    }
                }
            })
        });
    }
}
cart.init();
