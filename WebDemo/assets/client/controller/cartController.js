var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";

        });

        $('#btnPay').off('click').on('click', function () {
            window.location.href = "/payment";

        });

        $('#btnPay1').off('click').on('click', function () {
            window.location.href = "/paymentwithpaypal";

        });

        $('#btnUpdate').off('click').on('click', function () {
            var listProduct = $('.txtQuatity');
            var cartList = [];
            $.each(listProduct, function (i, item) {
                cartList.push({
                    Quatity: $(item).val(),
                    Product: {
                    ID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/Cart/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart-product";
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
                        window.location.href = "/cart-product";
                    }
                }
            })

        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            $.ajax({
                data: {id:$(this).data('id')},
                url: '/Cart/Delete',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/cart-product";
                    }
                }
            })

        });
    }
}
cart.init(); 